using System;
using System.Collections.Generic;
using System.Linq;

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

        void UpdateMinMaxXY()
        {
            this.minX = this.matrix.Keys.Min();
            this.maxX = this.matrix.Keys.Max();
            foreach (var xDict in this.matrix.Values)
            {
                this.minY = Math.Min(this.minY, xDict.Keys.Min());
                this.maxY = Math.Max(this.maxY, xDict.Keys.Max());
            }
        }

        public T[] AllItems()
        {
            var all = new List<T>();
            foreach (var i in this.matrix)
                all.AddRange(i.Value.Values);
            return all.ToArray();
        }

        public bool Remove(int x, int y)
        {
            if (this.matrix.ContainsKey(x) && this.matrix[x].Remove(y))
            {
                if (this.matrix[x].Count == 0)
                    this.matrix.Remove(x);
                UpdateMinMaxXY();
                return true;
            }
            return false;
        }

        public bool IsEmpty()
        {
            return this.matrix.Count == 0;
        }
    }
}