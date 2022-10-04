using System;

namespace Behavioral.State
{

    interface IPass
    {
        string Pass();
    }

    class AutomatePass : IPass
    {
        private int incomingMark;

        public AutomatePass(int incomingMark)
        {
            this.incomingMark = incomingMark;
        }
        public string Pass()
        {
            if (incomingMark >= 90)
                return "A";
            if (incomingMark >= 82)
                return "B";
            //..
            return "E";
        }
    }

    class StandartPass : IPass
    {
        public string Pass()
        {
            Console.WriteLine("Take a examination ticket... ");
            return "E";
        }
    }

    class ExclusionPass : IPass
    {
        public string Pass()
        {
            Console.WriteLine("Non-admission to the exam");
            return "F";
        }
    }
    class SubjectMark
    {
        public string Name { get; set; }
        private IPass Exam;
        private int _Rating;
        public int Rating
        {
            get
            {
                return this._Rating;
            }
            set
            {
                this._Rating = value;
                if (0 <= value && value <= 34)
                {
                    this.Exam = new ExclusionPass();
                }
                else if (35 <= value && value <= 59)
                {
                    this.Exam = new StandartPass();
                }
                else if (60 <= value && value <= 100)
                {
                    this.Exam = new AutomatePass(value);
                }
            }
        }

        public SubjectMark(string Name, int Rating)
        {
            this.Name = Name;
            this.Rating = Rating;
        }

        public void Pass()
        {
            Console.WriteLine($"Passing exam {this.Name}");
            string mark = this.Exam.Pass();
            Console.WriteLine($"Your mark is {mark}");
        }

    }
}
