using System;
using System.Linq;

namespace MyML_Lib
{
    public class Vector<T> : Matrix<T>
    {
        public Vector(int row) : base(row, 1) {
        }

        public Vector(T[] arr, int row) : base(arr, row, 1) {
        }

        public Vector(int row,  T val) : base(row, 1, val) {
            
        }

        public Vector(int row, double min, double max) : base(row, 1, min, max)
        {
            
        }

        public T this[int i]
        {
            get => Data[i];
            set => Data[i] = value;
        }
    }
}