using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Common
{
    public class Employee : Person
    {
        public string Position { get; set; }

        public Employee(string name, int age, string position)
              : base(name, age)
        {
            Position = position;
        }

        public override void PrintInfo()
        {
            base.PrintInfo();
            Console.WriteLine($"Посадаа: {Position}");
        }
    }
}
