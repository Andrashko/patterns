using System;
using System.Collections.Generic;

namespace Behavioral.Memento
{
    public interface IMementable
    {
        IMemento Save();
        void Restore (IMemento memento);
    } 
    class Originator: IMementable
    {
        private string _state;

        public Originator(string state)
        {
            _state = state;
            Console.WriteLine($"Originator: My initial state is: {state}");
        }

        public void DoSomething()
        {
            Console.WriteLine("Originator: I'm doing something important.");
            _state = GenerateRandomString(30);
            Console.WriteLine($"Originator: and my state has changed to: {_state}");
        }

        private string GenerateRandomString(int length = 10)
        {
            string allowedSymbols = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string result = string.Empty;

            for (int i = 0; i < length; i++)
                result += allowedSymbols[new Random().Next(0, allowedSymbols.Length)];

            return result;
        }

        public IMemento Save()
        {
            return new ConcreteMemento(_state);
        }

        public void Restore(IMemento memento)
        {
            if (!(memento is ConcreteMemento))
            {
                throw new Exception("Unknown memento class " + memento.ToString());
            }

            // _state = memento.GetState();
            _state = memento.State;
            Console.Write($"Originator: My state has changed to: {_state}");
        }
    }

    public interface IMemento
    {
        string GetName();

        // string GetState();
        string State { get; }

        DateTime GetDate();
    }


    class ConcreteMemento : IMemento
    {
        private string _state;

        private DateTime _date;

        public ConcreteMemento(string state)
        {
            _state = state;
            _date = DateTime.Now;
        }

        // public string GetState()
        // {
        //     return _state;
        // }

        public string State {
            get { return _state; }
        }

        public string GetName()
        {
            if (_state.Length <= 10)
                return $"{_date} / ({_state})";
            else
                return $"{_date} / ({_state.Substring(0, 9)})...";
        }

        public DateTime GetDate()
        {
            return _date;
        }
    }

    class Caretaker
    {
        private Stack<IMemento> _mementos = new Stack<IMemento>();

        private Originator _originator = null;

        public Caretaker(Originator originator)
        {
            _originator = originator;
        }

        public void Backup()
        {
            Console.WriteLine("\nCaretaker: Saving Originator's state...");
            _mementos.Push(_originator.Save());
        }

        public void Undo()
        {
            if (_mementos.Count == 0)
            {
                Console.WriteLine("Caretaker: There is no history to undo");
                return;
            }

            var memento = _mementos.Pop();

            Console.WriteLine("Caretaker: Restoring state to: " + memento.GetName());

            _originator.Restore(memento);
        }

        public void ShowHistory()
        {
            Console.WriteLine("Caretaker: Here's the list of mementos:");

            foreach (var memento in _mementos)
            {
                Console.WriteLine(memento.GetName());
            }
        }
    }
}