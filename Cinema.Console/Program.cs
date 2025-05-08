using Cinema.Infrastructure;
using Cinema.Infrastructure.Models;
using Cinema.Infrastructure.Services;
using Cinema.Nosql;
using Cinema.Nosql.Models;
using Cinema.Nosql.Repositories;
using Cinema.Nosql.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Cinema.ConsoleApp
{
    public class Program
    {
        public delegate void TicketAddedHandler(Ticket ticket);
        public static event TicketAddedHandler? OnTicketAdded;

        static async Task Main(string[] args)
        {
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;

            var services = new ServiceCollection();

            services.AddDbContext<CinemaContext>(options =>
                options.UseSqlite("Data Source=cinema.db"));

            var mongoSettings = new MongoDbSettings
            {
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = "CinemaDB"
            };
            services.AddSingleton<IOptions<MongoDbSettings>>(Options.Create(mongoSettings));
            services.AddSingleton<MongoDbInitializer>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ICrudServiceAsync<Movie>, EfCrudService<Movie>>();
            services.AddScoped<ICrudServiceAsync<Customer>, EfCrudService<Customer>>();
            services.AddScoped<ICrudServiceAsync<Ticket>, EfCrudService<Ticket>>();
            services.AddScoped<ICrudServiceAsync<Employee>, EfCrudService<Employee>>();

            services.AddScoped<IMongoRepository<MongoMovie>>(provider =>
                provider.GetRequiredService<MongoDbInitializer>().GetRepository<MongoMovie>("movies"));
            services.AddScoped<IMongoRepository<MongoTicket>>(provider =>
                provider.GetRequiredService<MongoDbInitializer>().GetRepository<MongoTicket>("tickets"));
            services.AddScoped<IMongoRepository<MongoCustomer>>(provider =>
                provider.GetRequiredService<MongoDbInitializer>().GetRepository<MongoCustomer>("customers"));
            services.AddScoped<MongoCinemaService>();

            var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider.CreateScope();
            var provider = scope.ServiceProvider;

            var movieService = provider.GetRequiredService<ICrudServiceAsync<Movie>>();
            var customerService = provider.GetRequiredService<ICrudServiceAsync<Customer>>();
            var ticketService = provider.GetRequiredService<ICrudServiceAsync<Ticket>>();
            var context = provider.GetRequiredService<CinemaContext>();

            var mongoService = provider.GetRequiredService<MongoCinemaService>();

            await context.Database.MigrateAsync();

            var customer = new Customer
            {
                Name = "Олександр Карішін",
                Age = 28,
                FavoriteGenre = "Драма"
            };

            await customerService.CreateAsync(customer);
            Console.WriteLine($"Додано клієнта: {customer.Name} (ID: {customer.Id})");

            var mongoCustomer = new MongoCustomer
            {
                OriginalId = customer.Id,
                Name = customer.Name,
                Age = customer.Age,
                FavoriteGenre = customer.FavoriteGenre
            };
            await mongoService.AddCustomerAsync(mongoCustomer);
            Console.WriteLine($"Додано клієнта в MongoDB: {mongoCustomer.Name} (ID: {mongoCustomer.Id})");

            if (!await context.Movies.AnyAsync())
            {
                var movies = new List<Movie>
                {
                    new() { Title = "Зелена миля", Genre = "Детектив", Year = 1999, Rating = 8.6 },
                    new() { Title = "1917", Genre = "Драма", Year = 2019, Rating = 8.2 },
                    new() { Title = "На Західному фронті без змін", Genre = "Драма", Year = 2022, Rating = 7.8 },
                    new() { Title = "Зелена книга", Genre = "Комедія", Year = 2018, Rating = 8.2 },
                    new() { Title = "Ножі наголо", Genre = "Детектив", Year = 2019, Rating = 7.9 },
                    new() { Title = "Пастка", Genre = "Трилер", Year = 2017, Rating = 5.3 }
                };

                foreach (var movie in movies)
                {
                    await movieService.CreateAsync(movie);

                    var mongoMovie = new MongoMovie
                    {
                        OriginalId = movie.Id,
                        Title = movie.Title,
                        Genre = movie.Genre,
                        Year = movie.Year,
                        Rating = movie.Rating
                    };
                    await mongoService.AddMovieAsync(mongoMovie);
                }

                Console.WriteLine("Фільми додано в бази даних (SQL та MongoDB).");
            }

            var allMovies = await movieService.ReadAllAsync();
            var tickets = new List<Ticket>();

            foreach (var movie in allMovies)
            {
                var ticket = new Ticket
                {
                    MovieId = movie.Id,
                    ShowTime = DateTime.Now.AddHours(2),
                    Price = 100 + new Random().Next(10, 50),
                    CustomerId = customer.Id
                };

                await ticketService.CreateAsync(ticket);
                tickets.Add(ticket);
                OnTicketAdded?.Invoke(ticket);

                var mongoTicket = new MongoTicket
                {
                    OriginalId = ticket.Id,
                    MovieId = (await mongoService.GetAllMoviesAsync())
                        .First(m => m.OriginalId == movie.Id).Id,
                    ShowTime = ticket.ShowTime,
                    Price = ticket.Price,
                    CustomerId = mongoCustomer.Id
                };
                await mongoService.AddTicketAsync(mongoTicket);
                await mongoService.AddTicketToCustomerAsync(mongoCustomer.Id, mongoTicket.Id);
            }

            Console.WriteLine("\nКвитки успішно збережено в бази даних (SQL та MongoDB)!");

            Console.WriteLine("\nСписок фільмів з SQL бази даних:\n");
            foreach (var movie in await movieService.ReadAllAsync())
            {
                Console.WriteLine($"{movie.Title} ({movie.Year}) - {movie.Genre}, Рейтинг: {movie.Rating}/10");
            }

            Console.WriteLine("\nСписок фільмів з MongoDB:\n");
            foreach (var movie in await mongoService.GetAllMoviesAsync())
            {
                Console.WriteLine($"{movie.Title} ({movie.Year}) - {movie.Genre}, Рейтинг: {movie.Rating}/10");
            }

            OnTicketAdded += ticket =>
            {
                Console.WriteLine($"\nДодано новий квиток: {ticket}");
            };

            Console.WriteLine("\nСписок квитків з SQL:\n");
            var allTickets = await context.Tickets.Include(t => t.Movie).ToListAsync();
            foreach (var ticket in allTickets)
            {
                Console.WriteLine($"{ticket.Movie.Title} | {ticket.ShowTime} | {ticket.Price} грн");
            }

            Console.WriteLine("\nСписок квитків з MongoDB:\n");
            var mongoTickets = await mongoService.GetAllTicketsAsync();
            foreach (var ticket in mongoTickets)
            {
                var movie = (await mongoService.GetAllMoviesAsync()).First(m => m.Id == ticket.MovieId);
                Console.WriteLine($"{movie.Title} | {ticket.ShowTime} | {ticket.Price} грн");
            }

            double total = allTickets.Sum(t => t.Price);
            Console.WriteLine($"\nЗагальна сума продажів: {total} грн");

            Console.ReadLine();
        }
    }
}