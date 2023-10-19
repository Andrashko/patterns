using System;
using System.Collections.Generic;

namespace Behavioral.Observer
{

    class DefaultSubject : IObservable<int>
    {
        private int _state = 0;
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
        private List<IObserver<int>> Observers = new List<IObserver<int>>();

        public IDisposable Subscribe(IObserver<int> observer)
        {
            if (!Observers.Contains(observer))
                Observers.Add(observer);
            return new Unsubscriber(Observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private List<IObserver<int>> _observers;
            private IObserver<int> _observer;

            public Unsubscriber(List<IObserver<int>> observers, IObserver<int> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }

        public void Unsubscribe(IObserver<int> observer)
        {
            observer.OnCompleted();
        }

        private void Notify()
        {
            foreach (var observer in Observers)
            {
                observer.OnNext(State);
            }
        }

        /// <summary>Змінює стан обєкту на випадкове ціле число від 0 до 9</summary>
        public void GenerateRandomState()
        {
            State = new Random().Next(0, 10);
        }
    }

    abstract class BaseObserver : IObserver<int>
    {
        private IDisposable unsubscriber;
        abstract public void OnNext(int value);

        public void OnError(Exception e)
        {
            Console.WriteLine(e.Message);
        }

        public void OnCompleted()
        {
            unsubscriber.Dispose();
        }

        public void Subscribe(IObservable<int> provider)
        {
            unsubscriber = provider.Subscribe(this);
        }
    }

    class ConsoleLogDefaultObserver : BaseObserver
    {
        public override void OnNext(int value)
        {
            Console.WriteLine($"Нове значення стану: {value}");
        }
    }

    class EvenDefaultObserver : BaseObserver
    {
        public override void OnNext(int value)
        {
            if (value % 2 == 0)
            {
                Console.WriteLine("Парне значення стану");
            }
        }
    }

    class CounterDefaultObserver : BaseObserver
    {

        private int Count = 0;
        private Predicate<int> Condition;

        /// <summary>Передаємо умову як предикат</summary>
        public CounterDefaultObserver(Predicate<int> Condidtion): base()
        {
            this.Condition = Condidtion;
        }
        public override void OnNext(int value)
        {
            if (Condition(value))
            {
                Count++;
                Console.WriteLine($"Стан задовільнив умову {Count} разів");
            }
        }
    }

}