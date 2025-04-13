using Cinema.Common;
using System;
using System.Collections.Generic;

namespace Cinema.Console
{
    internal class Program
    {
        public delegate void TicketAddedHandler(Ticket ticket);
        public static event TicketAddedHandler? OnTicketAdded;
        static void Main(string[] args)
        {
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;

            var movieService = new CrudService<Movie>();

            movieService.Create(new Movie("Зелена миля", "Детектив", 1999, 8.6));
            movieService.Create(new Movie("1917", "Драма", 2019, 8.2));
            movieService.Create(new Movie("На Західному фронті без змін", "Драма", 2022, 7.8));
            movieService.Create(new Movie("Зелена книга", "Комедія", 2018, 8.2));
            movieService.Create(new Movie("Ножі наголо", "Детектив", 2019, 7.9));
            movieService.Create(new Movie("Пастка", "Трилери", 2017, 5.3));

            string filePath = "movies.json";

            movieService.Save(filePath);
            System.Console.WriteLine($"\nДані збережено у файл: {filePath}");

            System.Console.WriteLine("\nПісля очищення:");
            var emptyService = new CrudService<Movie>();
            foreach (var m in emptyService.ReadAll())
            {
                System.Console.WriteLine(m); 
            }

            emptyService.Load(filePath);
            System.Console.WriteLine("\nПісля завантаження з файлу:");
            foreach (var m in emptyService.ReadAll())
            {
                System.Console.WriteLine(m);
            }


            System.Console.WriteLine("Список фільмів\n");

            foreach (var movie in movieService.ReadAll())
            {
                System.Console.WriteLine(movie);
            }

            List<Ticket> tickets = new List<Ticket>();

            OnTicketAdded += ticket =>
            {
                System.Console.WriteLine($"\nДодано новий квиток: {ticket}");
            };

            AddTicket(tickets, new Ticket("Зелена миля", DateTime.Now.AddHours(2), 120));
            AddTicket(tickets, new Ticket("1917", DateTime.Now.AddHours(3), 100));
            AddTicket(tickets, new Ticket("На Західному фронті без змін", DateTime.Now.AddHours(4), 110));
            AddTicket(tickets, new Ticket("Зелена книга", DateTime.Now.AddHours(4), 110));
            AddTicket(tickets, new Ticket("Ножі наголо", DateTime.Now.AddHours(4), 110));
            AddTicket(tickets, new Ticket("Пастка", DateTime.Now.AddHours(4), 110));

            System.Console.WriteLine("\nСписок квитків:\n");
            foreach (var ticket in tickets)
            {
                System.Console.WriteLine(ticket);
            }

            System.Console.WriteLine($"\nЗагальна сума продажів: {tickets.CalculateTotalPrice()} грн");

            System.Console.ReadLine();
        }
            public static void AddTicket(List<Ticket> tickets, Ticket newTicket)
            {
            tickets.Add(newTicket);
            OnTicketAdded?.Invoke(newTicket);
            }
    }

}