/*
* при розробці прикладів використано матеріали
* https://refactoring.guru/ru/design-patterns/flyweight/csharp/example
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Structural.Flyweight
{
    public class Car
    {
        public string Owner { get; set; }
        public string Number { get; set; }
        public string Company { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
    }

    /*
    * Пристосуванець зберігає спільну частину стану (також відому як внутрішній стан), яка належеть декільком реальним обєктам. 
    * Пристосуванець приймє решту стану (зовнішній стан, унікальний для кожного обєкту) через параемтри методу
    */
    public class Flyweight
    {
        private Car _sharedState;

        public Flyweight(Car car)
        {
            this._sharedState = car;
        }

        public void Operation(Car uniqueState)
        {
            string s = JsonSerializer.Serialize(this._sharedState);
            string u = JsonSerializer.Serialize(uniqueState);
            Console.WriteLine($"Flyweight: Displaying shared {s} and unique {u} state.");
        }
    }


    /*
    * Фабрика пристосуванців створює обєкти та керує ними.
    * Вона забезпечує правильний розподіл внутрішнього стану.
    * При створенні пристосуванця фабрика повертає існуючий спільний стан чи створює новий.
    */
    public class FlyweightFactory
    {
        private List<Tuple<Flyweight, string>> flyweights = new List<Tuple<Flyweight, string>>();

        public FlyweightFactory(params Car[] args)
        {
            foreach (var elem in args)
            {
                flyweights.Add(new Tuple<Flyweight, 
                string>(new Flyweight(elem), 
                this.getKey(elem)));
            }
        }

        // Повертає текстовий геш стану .
        public string getKey(Car key)
        {
            return $"{key?.Company}_{key?.Model}";
        }

        // Повертає існуючий чи створює новий внутрішній стан пристосуванця
        public Flyweight GetFlyweight(Car sharedState)
        {
            string key = this.getKey(sharedState);

            if (flyweights.Where(t => t.Item2 == key)
            .Count() == 0)
            {
                Console.WriteLine("FlyweightFactory: Can't find a flyweight, creating new one.");
                this.flyweights.Add(new Tuple<Flyweight,
                 string>(new Flyweight(sharedState), key));
            }
            else
            {
                Console.WriteLine("FlyweightFactory: Reusing existing flyweight.");
            }
            return this.flyweights
            .Where(t => t.Item2 == key).FirstOrDefault().Item1;
        }

        public void listFlyweights()
        {
            var count = flyweights.Count;
            Console.WriteLine($"\nFlyweightFactory: I have {count} flyweights:");
            foreach (var flyweight in flyweights)
            {
                Console.WriteLine(flyweight.Item2);
            }
        }
    }
}