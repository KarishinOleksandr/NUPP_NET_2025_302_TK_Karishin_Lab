using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lab2rework.Interfaces;

namespace lab2rework.Models
{
    public class Bus1 : IIdentifiable1
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Model1 { get; set; }
        public int Capacity1 { get; set; }

        public static Bus1 CreateNew()
        {
            var rnd = new Random();
            return new Bus1
            {
                Model1 = $"Model-{rnd.Next(1000, 9999)}",
                Capacity1 = rnd.Next(1000, 2000)
            };
        }
    }
}
