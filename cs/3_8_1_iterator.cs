using System;

namespace Behavioral.Iterator
{
    public interface IIteraleCollection<T>
    {
        IIterator<T> Iterator { get; }
    }

    public interface IIterator<T>
    {
        T Next();
    }

    public class Human
    {
        public string Name;
        public int Age;
        public Human(string Name, int Age)
        {
            this.Name = Name;
            this.Age = Age;
        }

        public override string ToString()
        {
            return $"{Age} : {Name}";
        }
    }

    public class HumanCollection : IIteraleCollection<Human>
    {
        private IIterator<Human> _Iterator;
        public IIterator<Human> Iterator { get { return _Iterator; } }

        public HumanCollection(Human[] Collection)
        {
            this._Iterator = new ReverseIterator(Collection);
        }
    }

    public class ReverseIterator : IIterator<Human>
    {
        private Human[] Collection;

        private int Pos;
        public Human Next()
        {
            if (Pos <= 0)
                return null;
            return this.Collection[--this.Pos];
        }

        public ReverseIterator(Human[] Collection)
        {
            this.Collection = Collection.Clone() as Human[];
            Array.Sort(this.Collection, (p1, p2) => p1.Age - p2.Age);
            this.Pos = Collection.Length;
        }
    }
}