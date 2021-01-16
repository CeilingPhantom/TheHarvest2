using System;
using System.Collections.Generic;

namespace TheHarvest.Util
{
    public class BoundlessSparseMatrix<T>
    {
        Dictionary<int, Dictionary<int, T>> matrix = new Dictionary<int, Dictionary<int, T>>();

        int minX = 0;
        int maxX = 0;
        int minY = 0;
        int maxY = 0;

        public T this[int x, int y]
        {
            get
            {
                if (this.matrix.ContainsKey(x) && this.matrix[x].ContainsKey(y))
                    return this.matrix[x][y];
                return default(T);
            }
            set
            {
                if (!this.matrix.ContainsKey(x))
                    this.matrix[x] = new Dictionary<int, T>();
                this.matrix[x][y] = value;
                if (x < this.minX)
                    this.minX = x;
                else if (x > this.maxX)
                    this.maxX = x;
                if (y < this.minY)
                    this.minY = y;
                else if (y > this.maxY)
                    this.maxY = y;
            }
        }

        public int Width => Math.Abs(this.maxX - this.minX);
        public int Height => Math.Abs(this.maxY - this.minY);

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
            foreach (var i in this.matrix)
                all.AddRange(i.Value.Values);
            return all.ToArray();
        }

        public bool IsEmpty()
        {
            // once an item has been placed, it can only be replaced
            // so don't have to check for actual items
            return this.matrix.Count == 0;
        }
    }
}