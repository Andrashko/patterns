using System;

namespace Behavioral.PersonVisitor
{
    
    public interface IVisitable
    {
        void Accept(IVisitor visitor);
    }

    public abstract class Person : IVisitable
    {
        public string Name;
        public string Surname;

        public Person(string Name, string Surname)
        {
            this.Name = Name;
            this.Surname = Surname;
        }
        public abstract void Accept(IVisitor visitor);
    }

    public class Student : Person
    {
        public int Course;
        public Student(string Name, string Surname, int Course) : base(Name, Surname)
        {
            this.Course = Course;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.VisitStudent(this);
        }

        public string ToPrintString()
        {
            return $"{Name}, {Surname}, {Course}";
        }
        // public void Print()
        // {
        //     Console.WriteLine($"Студент {ToPrintString()}");
        // }

        // public void SayHi()
        // {
        //     Console.WriteLine($"Привіт, {Name}");
        // }
    }

    public class Professor : Person
    {
        public string Cathedra;
        public string SecondName;
        public Professor(string Name, string Surname, string SecondName, string Cathedra) : base(Name, Surname)
        {
            this.Cathedra = Cathedra;
            this.SecondName = SecondName;
        }
        public override void Accept(IVisitor visitor)
        {
            visitor.VisitProfessor(this);
        }

        public string ToPrintString()
        {
            return $"{Surname} {Name} {SecondName}, {Cathedra}";
        }
        // public void Print()
        // {
        //     Console.WriteLine($"Викладач {ToPrintString()}");
        // }

        // public void SayHi()
        // {
        //     Console.WriteLine($"Доброго дня, {Name} {SecondName}");
        // }
    }

    public interface IVisitor
    {
        void VisitStudent(Student student);
        void VisitProfessor(Professor professor);
    }

    public class Printer : IVisitor
    {
        public void VisitStudent(Student student)
        {
            Console.WriteLine("Друкую студента");
            Console.WriteLine(student.ToPrintString());
        }
        public void VisitProfessor(Professor professor)
        {
            Console.WriteLine("Друкую професора");
            Console.WriteLine(professor.ToPrintString());
        }
    }

    public class Hi : IVisitor
    {
        public void VisitStudent(Student student)
        {
            Console.WriteLine($"Привiт {student.Name}");
        }
        public void VisitProfessor(Professor professor)
        {
            Console.WriteLine($"Доброго дня, {professor.Name} {professor.SecondName}");
        }
    }

    
}