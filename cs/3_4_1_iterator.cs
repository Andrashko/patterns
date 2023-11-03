using System;

namespace Behavioral.Iterator
{
    public interface IIterableCollection<T>
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

    public class HumanCollection : IIterableCollection<Human>
    {
        private IIterator<Human> _iterator;
        public IIterator<Human> Iterator { get { return _iterator; } }

        public HumanCollection(Human[] Collection)
        {
            _iterator = new ReverseIterator(Collection);
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
            return Collection[--Pos];
        }

        public ReverseIterator(Human[] Collection)
        {
            this.Collection = Collection.Clone() as Human[];
            Array.Sort(this.Collection, (p1, p2) => p1.Age - p2.Age);
            Pos = Collection.Length;
        }
    }
}