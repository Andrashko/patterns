/*
* при розробці прикладів використано матеріали
* https://refactoring.guru/ru/design-patterns/flyweight/csharp/example
*/
using System;
using System.Collections.Generic;
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
    public class CarSharedState
    {
        public string Company { get; set; }
        public string Model { get; set; }

        public CarSharedState(Car car)
        {
            this.Company = car.Company;
            this.Model = car.Model;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }

    //зовнішній стан
    public class CarUniqueState
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

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }

    public class Flyweight
    {
        private CarSharedState _sharedState;
        private CarUniqueState _uniqueState;

        public Flyweight(Car car)
        {
            _sharedState = new CarSharedState(car);
            _uniqueState = new CarUniqueState(car);
        }

        public Flyweight(CarSharedState sharedState)
        {
            _sharedState = sharedState;
        }

        public void SetUniqueState(Car uniqueState)
        {
            _uniqueState = new CarUniqueState(uniqueState);
        }

        public override string ToString()
        {
            string s = JsonSerializer.Serialize(this._sharedState);
            string u = JsonSerializer.Serialize(this._uniqueState);
            return $"Flyweight: shared {s} and unique {u} state";
        }

        public Car GetStandartObject()
        {
            return new Car()
            {
                Company = _sharedState.Company,
                Model = _sharedState.Model,
                Owner = _uniqueState.Owner,
                Number = _uniqueState.Number,
                Color = _uniqueState.Color
            };
        }

        /*
        Також пристосуванець може мати  властивості з гетерами та сетерами для зовнішнього стану 
        і властивості з гетерами для внутрішнього стану. Сетери властивостей внутрішнього стану КАТЕГОРИЧНО ЗАБОРОНЕНІ
        */
        public string Owner
        {
            get
            {
                return _uniqueState.Owner;
            }
            set
            {
                _uniqueState.Owner = value;
            }
        }
        public string Number
        {
            get
            {
                return _uniqueState.Number;
            }
            set
            {
                _uniqueState.Number = value;
            }
        }
        public string Color
        {
            get
            {
                return _uniqueState.Color;
            }
            set
            {
                _uniqueState.Color = value;
            }
        }

        public string Model
        {
            get
            {
                return _sharedState.Model;
            }
        }

        public string Company
        {
            get
            {
                return _sharedState.Company;
            }
        }
    }


    /*
    * Фабрика пристосуванців створює обєкти та керує ними.
    * Вона забезпечує правильний розподіл внутрішнього стану.
    * При створенні пристосуванця фабрика повертає існуючий спільний стан чи створює новий.
    */
    public class FlyweightFactory
    {
        private Dictionary<string, CarSharedState> sharedStates = new Dictionary<string, CarSharedState>();

        public FlyweightFactory(params Car[] args)
        {
            foreach (var elem in args)
            {
                sharedStates.Add(getKey(elem), new CarSharedState(elem));
            }
        }

        // Повертає текстовий геш для спільного стану 
        private string getKey(Car key)
        {
            return $"{key?.Company}_{key?.Model}";
        }

        // Повертає існуючий чи створює новий внутрішній стан пристосуванця
        public Flyweight GetFlyweight(Car car)
        {
            string key = getKey(car);
            // якщо нема жодного спільного стану з вказаним ключем, то його потрібно створити 
            if (!sharedStates.ContainsKey(key))
            {
                // Console.WriteLine("FlyweightFactory: Can't find a flyweight, creating new one.");
                sharedStates.Add(key, new CarSharedState(car));
            }
            else
            {
                // Console.WriteLine("FlyweightFactory: Reusing existing flyweight.");
            }
            // встановлюємо спільний сатн пристосуванця із закешованого списку
            CarSharedState sharedState = sharedStates.GetValueOrDefault(key);
            Flyweight flyweight = new Flyweight(sharedState);
            // встановлюємо унікальний стан пристосуваннця 
            flyweight.SetUniqueState(car);
            return flyweight;
        }

        public void listSharedStates()
        {
            Console.WriteLine($"\nFlyweightFactory: I have {sharedStates.Count} shared states:");
            foreach (var sharedState in sharedStates)
            {
                Console.WriteLine(sharedState);
            }
        }
    }
}