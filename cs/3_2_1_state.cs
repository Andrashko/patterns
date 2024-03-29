using System;
using System.Collections.Generic;

namespace Behavioral.State
{

    interface IPass
    {
        string Pass(int rating = 0);
    }
    class AutomatePass : IPass
    {
        public string Pass(int rating)
        {
            if (rating >= 90)
                return "A";
            if (rating >= 82)
                return "B";
            if (rating >= 74)
                return "C";
            if (rating >= 64)
                return "D";
            return "E";
        }
    }

    class NormalPass : IPass
    {
        public string Pass(int rating = 0)
        {
            Console.WriteLine("Take a examination ticket... ");
            //...
            return "E";
        }
    }

    class ExclusionPass : IPass
    {
        public string Pass(int rating = 0)
        {
            Console.WriteLine("Non-admission to the exam");
            return "F";
        }
    }

    class SubjectMark
    {
        public string Name { get; set; }
        private IPass _passState;
        private int _rating;
        public int Rating
        {
            get
            {
                return _rating;
            }
            set
            {
                _passState = GetPassState(value);
                _rating = value;
            }
        }
        public static List<Tuple<Predicate<int>, IPass>> state_selectors = new List<Tuple<Predicate<int>, IPass>>() {
            new (
                rating => 60<=rating && rating<=100,
                new AutomatePass()
                ),
            new (
                rating => 35<=rating && rating<60,
                new NormalPass()
                ),
            new (
                rating => 0<=rating && rating<35,
                new ExclusionPass()
                ),
        };

        private IPass GetPassState(int value)
        {
            var state_selector = state_selectors.Find(s => s.Item1(value));
            if (state_selector == null)
                throw new Exception($"{value} is not valid");
            return state_selector.Item2;
        }

        public SubjectMark(string Name, int Rating)
        {
            this.Name = Name;
            this.Rating = Rating;
        }

        public void Pass()
        {
            Console.WriteLine("===========================");
            Console.WriteLine($"Passing exam {Name}");
            string mark = _passState.Pass(Rating);
            Console.WriteLine($"Your mark is {mark}");
        }

    }
}
