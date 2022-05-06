using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PudelkoLibrary
{
    public sealed class PudelkoEnum : IEnumerator
    {
        private readonly double[] _dimensions;
        private int _position = -1;

        public PudelkoEnum(double[] dimensions)
        {
            _dimensions = dimensions;
        }

        object IEnumerator.Current => Current;

        public double Current
        {
            get
            {
                try
                {
                    return _dimensions[_position];
                }
                catch(IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public bool MoveNext()
        {
            _position++;
            return (_position < _dimensions.Length);
        }

        public void Reset()
        {
            _position = -1;
        }
    }
}
