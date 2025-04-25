using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab2.Models;
using Lab2.Services;

namespace Lab2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var service = new InMemoryCrudService<Bus>("buses.json");

            var tasks = Enumerable.Range(0, 1000)
                .Select(_ => service.CreateAsync(Bus.CreateNew()))
                .ToArray();

            await Task.WhenAll(tasks);

            await service.SaveAsync();

            var allBuses = await service.ReadAllAsync();

            var min = allBuses.Min(b => b.Capacity);
            var max = allBuses.Max(b => b.Capacity);
            var avg = allBuses.Average(b => b.Capacity);

            Console.WriteLine($"Min Capacity: {min}");
            Console.WriteLine($"Max Capacity: {max}");
            Console.WriteLine($"Average Capacity: {avg:F2}");

            Console.WriteLine("Buses saved to file successfully.");
        }
    }
}
