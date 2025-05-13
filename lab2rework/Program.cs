using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lab2rework.Models;
using lab2rework.Services;

namespace lab2rework
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var service = new InMemoryCrudService1<Bus1>("buses.json");

            var tasks = Enumerable.Range(0, 1000)
                .Select(_ => service.CreateAsync1(Bus1.CreateNew()))
                .ToArray();

            await Task.WhenAll(tasks);

            await service.SaveAsync1();

            var allBuses = await service.ReadAllAsync1();

            var min = allBuses.Min(b => b.Capacity1);
            var max = allBuses.Max(b => b.Capacity1);
            var avg = allBuses.Average(b => b.Capacity1);

            Console.WriteLine($"Min Capacity: {min}");
            Console.WriteLine($"Max Capacity: {max}");
            Console.WriteLine($"Average Capacity: {avg:F2}");

            Console.WriteLine("Buses saved to file successfully.");
        }
    }
}
