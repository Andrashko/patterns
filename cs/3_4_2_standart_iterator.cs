using System;
using System.Collections;
using System.Collections.Generic;

namespace Behavioral.Iterator
{
    abstract class Iterator<Type> : IEnumerator<Type>
    {

        public abstract Type Current { get; }
        public abstract bool MoveNext();
        public abstract void Reset();
        public abstract void Dispose();
        object IEnumerator.Current
        => throw new NotImplementedException();
    }

    abstract class IteratorAggregate<Type> : IEnumerable<Type>
    {
        public abstract IEnumerator<Type> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        => throw new NotImplementedException();

    }

    class AlphabeticalOrderIterator : Iterator<Human>
    {
        private HumanStandartCollection _collection;
        private int _position;
        private bool _reverse;

        public AlphabeticalOrderIterator(HumanStandartCollection collection, bool reverse = false)
        {
            _collection = collection;
            _reverse = reverse;
            Array.Sort(
                _collection.getItems(),
                (human1, human2) => String.Compare(human1.Name, human2.Name)
            );
            Reset();
        }

        public override Human Current
        {
            get
            {
                return _collection.getItems()[_position];
            }
        }
        private int GetChange()
        {
            if (_reverse)
                return -1;
            return 1;
        }

        public override bool MoveNext()
        {
            int updatedPosition = _position + GetChange();

            if (updatedPosition >= 0 && updatedPosition < _collection.Count)
            {
                _position = updatedPosition;
                return true;
            }

            return false;
        }

        public override void Reset()
        {
            if (_reverse)
                _position = _collection.Count;
            else
                _position = -1;
        }

        public override void Dispose()
        {
            // _collection = null;
        }
    }


    class HumanStandartCollection : IteratorAggregate<Human>
    {
        private Human[] _collection;

        private bool _direction = false;

        public HumanStandartCollection(Human[] humans)
        {
            _collection = humans.Clone() as Human[];
        }

        public void ReverseDirection()
        {
            _direction = !_direction;
        }

        public Human[] getItems()
        {
            return _collection;
        }

        public int Count { get { return _collection.Length; } }

        public override IEnumerator<Human> GetEnumerator()
        {
            return new AlphabeticalOrderIterator(this, _direction);
        }
    }


}