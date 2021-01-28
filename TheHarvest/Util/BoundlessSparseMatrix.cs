using System;
using System.Collections.Generic;
using System.Linq;

namespace TheHarvest.Util
{
    public class BoundlessSparseMatrix<T>
    {
        Dictionary<int, Dictionary<int, T>> matrix = new Dictionary<int, Dictionary<int, T>>();

        int? minX = 0;
        int? maxX = 0;
        int? minY = 0;
        int? maxY = 0;

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
                if (!this.minX.HasValue || x < this.minX)
                    this.minX = x;
                if (!this.maxX.HasValue || x > this.maxX)
                    this.maxX = x;
                if (!this.minY.HasValue || y < this.minY)
                    this.minY = y;
                if (!this.minY.HasValue || y > this.maxY)
                    this.maxY = y;
            }
        }

        public int Width => this.minX.HasValue ? Math.Abs(this.maxX.Value - this.minX.Value) : 0;
        public int Height => this.minY.HasValue ? Math.Abs(this.maxY.Value - this.minY.Value) : 0;

        public BoundlessSparseMatrix()
        {}

        public BoundlessSparseMatrix(int initWidth, int initHeight, Func<T> initVal)
        {
            for (var i = 0; i < initWidth; ++i)
                for (var j = 0; j < initHeight; ++j)
                    this[i, j] = initVal();
        }

        public T[] AllValues()
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

        void UpdateMinMaxXY()
        {
            if (this.matrix.Keys.Count() == 0)
            {
                this.minX = null;
                this.maxX = null;
                this.minY = null;
                this.maxY = null;
                return;
            }
            this.minX = this.matrix.Keys.Min();
            this.maxX = this.matrix.Keys.Max();
            foreach (var xDict in this.matrix.Values)
            {
                this.minY = Math.Min(this.minY.Value, xDict.Keys.Min());
                this.maxY = Math.Max(this.maxY.Value, xDict.Keys.Max());
            }
        }

        public bool IsEmpty()
        {
            return this.matrix.Count == 0;
        }
    }
}