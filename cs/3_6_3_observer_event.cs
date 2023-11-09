using System;


namespace Behavioral.Observer
{
    interface IChangeStateEventable
    {
        event EventHandler<IntEventArgs> ChangeStateEvent;
    }
    class IntEventArgs : EventArgs
    {
        public int Value;
        public IntEventArgs(int value)
        {
            Value = value;
        }
    }
    class EventSubject : IChangeStateEventable
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
                ChangeStateEvent.Invoke(this, new IntEventArgs(value));
            }
        }
        /// <summary>Змінює стан обєкту на випадкове ціле число від 0 до 9</summary>
        public void GenerateRandomState()
        {
            State = new Random().Next(0, 10);
        }

        public event EventHandler<IntEventArgs> ChangeStateEvent = delegate { };

        public EventHandler<IntEventArgs> OnCahngeState
        {
            set
            {
                ChangeStateEvent = value;
            }
        }
    }

    class EventHandlers
    {
        public static void Log(object Sender, IntEventArgs e)
        {
            Console.WriteLine($"Нове значення стану:  {e.Value}");
        }

        public static void LogEven(object Sender, IntEventArgs e)
        {
            if (e.Value % 2 == 0)
            {
                Console.WriteLine("Парне значення стану");
            }
        }
    }

    /// <summary>Підраховує кількість станів, що задовільняють певній умові</summary>
    class CounterEventObserver
    {
        private int Count = 0;
        private Predicate<int> Condition;

        /// <summary>Передаємо умову як предикат</summary>
        public CounterEventObserver(Predicate<int> Condidtion)
        {
            this.Condition = Condidtion;
        }

        private void Handler(object Sender, IntEventArgs e)
        {
            if (Condition(e.Value))
            {
                Count++;
                Console.WriteLine($"Стан задовільнив умову {Count} разів");
            }
        }
        public void Subscribe(IChangeStateEventable subject)
        {
            subject.ChangeStateEvent += Handler;
        }
    }


}