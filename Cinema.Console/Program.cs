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
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Cinema.ConsoleApp
{
    public class Program
    {
        public delegate void TicketAddedHandler(Ticket ticket);
        public static event TicketAddedHandler? OnTicketAdded;

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var services = new ServiceCollection();

            services.AddDbContext<CinemaContext>(options =>
                options.UseSqlite("Data Source=cinema.db"));

            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
            var mongoSettings = new MongoDbSettings
            {
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = "CinemaDB"
            };
            services.AddSingleton(Options.Create(mongoSettings));
            services.AddSingleton<MongoDbInitializer>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ICrudServiceAsync<Movie>, EfCrudService<Movie>>();
            services.AddScoped<ICrudServiceAsync<Customer>, EfCrudService<Customer>>();
            services.AddScoped<ICrudServiceAsync<Ticket>, EfCrudService<Ticket>>();
            services.AddScoped<ICrudServiceAsync<Employee>, EfCrudService<Employee>>();

            services.AddScoped<IMongoRepository<MongoMovie>>(p =>
                p.GetRequiredService<MongoDbInitializer>().GetRepository<MongoMovie>("movies"));
            services.AddScoped<IMongoRepository<MongoTicket>>(p =>
                p.GetRequiredService<MongoDbInitializer>().GetRepository<MongoTicket>("tickets"));
            services.AddScoped<IMongoRepository<MongoCustomer>>(p =>
                p.GetRequiredService<MongoDbInitializer>().GetRepository<MongoCustomer>("customers"));
            services.AddScoped<MongoCinemaService>();

            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var provider = scope.ServiceProvider;

            var context = provider.GetRequiredService<CinemaContext>();
            await context.Database.MigrateAsync();

            var movieService = provider.GetRequiredService<ICrudServiceAsync<Movie>>();
            var customerService = provider.GetRequiredService<ICrudServiceAsync<Customer>>();
            var ticketService = provider.GetRequiredService<ICrudServiceAsync<Ticket>>();
            var mongoService = provider.GetRequiredService<MongoCinemaService>();

            var customer = new Customer
            {
                Name = "Олександр Карішін",
                Age = 28,
                FavoriteGenre = "Драма"
            };
            await customerService.CreateAsync(customer);

            var mongoCustomer = new MongoCustomer
            {
                OriginalId = customer.Id,
                Name = customer.Name,
                Age = customer.Age,
                FavoriteGenre = customer.FavoriteGenre
            };
            await mongoService.AddCustomerAsync(mongoCustomer);

            var initialMovies = new List<Movie>
            {
                new() { Title = "Зелена миля", Genre = "Детектив", Year = 1999, Rating = 8.6 },
                new() { Title = "1917", Genre = "Драма", Year = 2019, Rating = 8.2 },
                new() { Title = "На Західному фронті без змін", Genre = "Драма", Year = 2022, Rating = 7.8 },
                new() { Title = "Зелена книга", Genre = "Комедія", Year = 2018, Rating = 8.2 },
                new() { Title = "Ножі наголо", Genre = "Детектив", Year = 2019, Rating = 7.9 },
                new() { Title = "Пастка", Genre = "Трилер", Year = 2017, Rating = 5.3 }
            };

            var existingSqlMovies = await movieService.ReadAllAsync();
            var existingMongoMovies = await mongoService.GetAllMoviesAsync();

            var existingSqlSet = existingSqlMovies
                .Select(m => (m.Title, m.Year))
                .ToHashSet();

            foreach (var movie in initialMovies)
            {
                Movie sqlMovie;
                if (!existingSqlSet.Contains((movie.Title, movie.Year)))
                {
                    await movieService.CreateAsync(movie);
                    sqlMovie = movie;
                }
                else
                {
                    sqlMovie = existingSqlMovies.First(m => m.Title == movie.Title && m.Year == movie.Year);
                }

                var mongoMovie = existingMongoMovies
                    .FirstOrDefault(m => m.OriginalId == sqlMovie.Id);

                if (mongoMovie == null)
                {
                    mongoMovie = new MongoMovie
                    {
                        OriginalId = sqlMovie.Id,
                        Title = sqlMovie.Title,
                        Genre = sqlMovie.Genre,
                        Year = sqlMovie.Year,
                        Rating = sqlMovie.Rating
                    };
                    await mongoService.AddMovieAsync(mongoMovie);
                }

                var ticket = new Ticket
                {
                    MovieId = sqlMovie.Id,
                    ShowTime = DateTime.Now.AddHours(2),
                    Price = 100 + new Random().Next(10, 50),
                    CustomerId = customer.Id
                };
                await ticketService.CreateAsync(ticket);

                var mongoTicket = new MongoTicket
                {
                    OriginalId = ticket.Id,
                    MovieId = mongoMovie.Id,
                    ShowTime = ticket.ShowTime,
                    Price = ticket.Price,
                    CustomerId = mongoCustomer.Id
                };
                await mongoService.AddTicketAsync(mongoTicket);
                await mongoService.AddTicketToCustomerAsync(mongoCustomer.Id, mongoTicket.Id);

                OnTicketAdded?.Invoke(ticket);
            }

            Console.WriteLine("\nСписок фільмів:");
            var distinctMovies = (await movieService.ReadAllAsync())
                .GroupBy(m => new { m.Title, m.Year })
                .Select(g => g.First())
                .ToList();

            foreach (var movie in distinctMovies)
            {
                Console.WriteLine($"{movie.Title} ({movie.Year}) - {movie.Genre}, Рейтинг: {movie.Rating}/10");
            }

            Console.WriteLine("\nСписок квитків:");
            var allTickets = await context.Tickets.Include(t => t.Movie).ToListAsync();
            foreach (var ticket in allTickets)
            {
                Console.WriteLine($"{ticket.Movie.Title} | {ticket.ShowTime} | {ticket.Price} грн");
            }

            double total = allTickets.Sum(t => t.Price);
            Console.WriteLine($"\nЗагальна сума продажів: {total} грн");

            Console.ReadLine();
        }
    }
}
