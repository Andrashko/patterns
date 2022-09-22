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
    //вихідний стан
    public class Car
    {
        public string Owner { get; set; }
        public string Number { get; set; }
        public string Company { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
    /*
    * Пристосуванець зберігає спільну частину стану (також відому як внутрішній стан), яка належеть декільком реальним обєктам. 
    * Пристосуванець приймє решту стану (зовнішній стан, унікальний для кожного обєкту) через параемтри методу
    * Також пристосуванець  може повернути оригінальний об'єкт 
    */


    //внутрішній стан
    class CarSharedState
    {
        public string Company { get; set; }
        public string Model { get; set; }

        public CarSharedState(Car car)
        {
            this.Company = car.Company;
            this.Model = car.Model;
        }
    }

    //зовнішній стан
    class CarUniqueState
    {
        public string Owner { get; set; }
        public string Number { get; set; }
        public string Color { get; set; }

        public CarUniqueState(Car car)
        {
            this.Owner = car.Owner;
            this.Number = car.Number;
            this.Color = car.Color;
        }
    }

    public class Flyweight
    {
        private CarSharedState _sharedState;
        private CarUniqueState _uniqueState;

        public Flyweight(Car car)
        {
            this._sharedState = new CarSharedState(car);
            this._uniqueState = new CarUniqueState(car);
        }

        public void SetUniqueState(Car uniqueState)
        {
            this._uniqueState = new CarUniqueState(uniqueState);
        }

        public void Print()
        {
            string s = JsonSerializer.Serialize(this._sharedState);
            string u = JsonSerializer.Serialize(this._uniqueState);
            Console.WriteLine($"Flyweight: Displaying shared {s} and unique {u} state.");
        }

        public Car GetStandartObject()
        {
            return new Car()
            {
                Company = this._sharedState.Company,
                Model = this._sharedState.Model,
                Owner = this._uniqueState.Owner,
                Number = this._uniqueState.Number,
                Color = this._uniqueState.Color
            };
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
                flyweights.Add(new Tuple<Flyweight, string>
                (new Flyweight(elem), this.getKey(elem)));
            }
        }

        // Повертає текстовий геш для спільного стану 
        public string getKey(Car key)
        {
            return $"{key?.Company}_{key?.Model}";
        }

        // Повертає існуючий чи створює новий внутрішній стан пристосуванця
        public Flyweight GetFlyweight(Car sharedState)
        {
            string key = this.getKey(sharedState);
            // якщо нема жодного спільного стану з вказаним ключем, то його потрібно створити 
            if (!flyweights.Any(t => t.Item2 == key))
            {
                Console.WriteLine("FlyweightFactory: Can't find a flyweight, creating new one.");
                this.flyweights.Add(new Tuple<Flyweight, string>(
                    new Flyweight(sharedState), key)
                );
            }
            else
            {
                Console.WriteLine("FlyweightFactory: Reusing existing flyweight.");
            }
            return this.flyweights.Where(t => t.Item2 == key).FirstOrDefault().Item1;
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