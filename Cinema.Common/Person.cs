using System;

namespace Cinema.Common
{
    public abstract class Person
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public static int TotalPeople = 0;

        static Person()
        {
            Console.WriteLine("Створено клас Person");
        }

        public Person(string name, int age)
        {
            ID = Guid.NewGuid();
            Name = name;
            Age = age;
            TotalPeople++;
        }

        public virtual void PrintInfo()
        {
            Console.WriteLine($"Ім'я: {Name}, Вік: {Age}");
        }
    }
}
