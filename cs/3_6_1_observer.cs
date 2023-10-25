using System;
using System.Collections.Generic;

namespace Behavioral.Observer
{
    /// <summary>Інтерфейс спостерігача</summary>
    interface ICustomObserver
    {
        /// <summary>Реакція спостерігача на зміну стану субєкта </summary>
        /// <param name="subject">Субєкт, за яким ведеться спостереження</param>
        void Update(ISubject subject);
    }

    /// <summary>Інтерфейс субєкта, за яким можна вести спостереження</summary>
    interface ISubject
    {
        /// <summary>підписати спостерігача</summary>
        void Attach(ICustomObserver observer);
        /// <summary>відписати  спостерігача</summary>
        void Detach(ICustomObserver observer);
        /// <summary>повідомити всіх підписаних спостерігачів</summary>
        void Notify();
        /// <value>Гетерт для того, щоб спостерігач міг отримати стан субєкта</value>
        int State { get; }
    }

    class Subject : ISubject
    {
        /// <summary> Сатн субєкта визначаємо цілим числом. Стан повинен бути публічно доступний для спостерігача</summary>
        private int _state = 0;

        /// <value> При зміні стану повідомляємо спостерігачів </value>
        public int State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                Notify();
            }
        }
        private List<ICustomObserver> Observers = new List<ICustomObserver>();
        public void Attach(ICustomObserver observer)
        {
            Observers.Add(observer);
        }
        public void Detach(ICustomObserver observer)
        {
            Observers.Remove(observer);
        }
        public void Notify()
        {
            foreach (var observer in Observers)
            {
                observer.Update(this);
            }
        }
        /// <summary>Змінює стан обєкту на випадкове ціле число від 0 до 9</summary>
        public void GenerateRandomState()
        {
            State = new Random().Next(0, 10);
        }
    }

    /// <summary>Спостерігач виводить в консоль кожну зміну стану</summary>
    class ConsoleLogObserver : ICustomObserver
    {
        public void Update(ISubject subject)
        {
            Console.WriteLine($"Нове значення стану: {subject.State}");
        }
    }



    /// <summary>Спостерігач, який виводить повідомлення про парний стан</summary>
    class EvenObserver : ICustomObserver
    {
        public void Update(ISubject subject)
        {
            if (subject.State % 2 == 0)
            {
                Console.WriteLine("Парне значення стану");
            }
        }
    }
    /// <summary>Підраховує кількість станів, що задовільняють певній умові</summary>
    class CounterObserver : ICustomObserver
    {

        private int Count = 0;
        private Predicate<int> Condition;

        /// <summary>Передаємо умову як предикат</summary>
        public CounterObserver(Predicate<int> Condidtion)
        {
            this.Condition = Condidtion;
        }
        public void Update(ISubject subject)
        {
            if (Condition(subject.State))
            {
                Count++;
                Console.WriteLine($"Стан задовільнив умову {Count} разів");
            }
        }
    }

}