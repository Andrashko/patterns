using System;
using System.Collections;
using System.Collections.Generic;

namespace Behavioral.Iterator
{
    class IterableCollection<Type> : IEnumerable<Type>
    {
        private Type[] _array;
        private int _position;

        private bool _reverse = false;



        public IterableCollection(Type[] array, bool reverse = false)
        {
            _array = array;
            _reverse = reverse;
            Reset();
        }

        private void Reset()
        {
            if (_reverse)
                _position = _array.Length - 1;
            else
                _position = 0;
        }

        private int GetChange()
        {
            if (_reverse)
                return -1;
            return 1;
        }

        public void ReverseDirection()
        {
            _reverse = !_reverse;
            Reset();
        }
        public IEnumerator<Type> GetEnumerator()
        {
            while (_position < _array.Length && _position >= 0)
            {
                yield return _array[_position];
                _position += GetChange();
            }
            Reset();
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}