using System;
using System.Collections.Generic;

namespace TheHarvest.Util
{
    public class BoundlessSparseMatrix<T>
    {
        Dictionary<int, Dictionary<int, T>> matrix = new Dictionary<int, Dictionary<int, T>>();

        public T this[int x, int y]
        {
            get
            {
                if (matrix.ContainsKey(x) && matrix[x].ContainsKey(y))
                    return matrix[x][y];
                return default(T);
            }
            set
            {
                if (!matrix.ContainsKey(x))
                    matrix[x] = new Dictionary<int, T>();
                matrix[x][y] = value;
            }
        }

        public BoundlessSparseMatrix()
        {}

        public BoundlessSparseMatrix(int initWidth, int initHeight, Func<T> initVal)
        {
            for (var i = 0; i < initWidth; ++i)
                for (var j = 0; j < initHeight; ++j)
                    this[i, j] = initVal();
        }

        public T[] AllItems()
        {
            var all = new List<T>();
            foreach (var i in matrix)
                all.AddRange(i.Value.Values);
            return all.ToArray();
        }

        public bool IsEmpty()
        {
            // once an item has been placed, it can only be replaced
            // so don't have to check for actual items
            return matrix.Count == 0;
        }
    }
}