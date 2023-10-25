using System.Collections;
using System.Collections.Generic;

namespace Behavioral.Iterator
{
    abstract class Iterator : IEnumerator
    {
        object IEnumerator.Current => Current();
        public abstract int Key();
        public abstract object Current();
        public abstract bool MoveNext();
        public abstract void Reset();
    }

    abstract class IteratorAggregate : IEnumerable
    {
        public abstract IEnumerator GetEnumerator();
    }

    class AlphabeticalOrderIterator : Iterator
    {
        private WordsCollection _collection;
        private int _position = -1;
        private bool _reverse = false;

        public AlphabeticalOrderIterator(WordsCollection collection, bool reverse = false)
        {
            _collection = collection;
            _reverse = reverse;
            if (reverse)
            {
                _position = collection.Count;
            }
        }

        public override object Current()
        {
            return _collection.getItems()[_position];
        }

        public override int Key()
        {
            return _position;
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
            else
            {
                return false;
            }
        }

        public override void Reset()
        {
            if (_reverse)
                _position = _collection.Count - 1;
            else
                _position = 0;
        }
    }


    class WordsCollection : IteratorAggregate
    {
        private string[] _collection;

        private bool _direction = false;

        public WordsCollection(string[] words)
        {
            _collection = words;
        }

        public void ReverseDirection()
        {
            _direction = !_direction;
        }

        public string[] getItems()
        {
            return _collection;
        }

        public int Count { get { return _collection.Length; } }

        public override IEnumerator GetEnumerator()
        {
            return new AlphabeticalOrderIterator(this, _direction);
        }
    }


}