using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathos
{
    class SPDRect
    {
        /// <summary>
        /// Rectangle algorithm used by this module.
        /// </summary>
        public int left;
        public int top;
        public int right;
        public int bottom;

        public SPDRect()
        {
            new SPDRect(0, 0, 0, 0);
        }
        public SPDRect(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        public SPDRect(SPDRect rect)
        {
            new SPDRect(rect.left, rect.top, rect.right, rect.bottom);
        }

        public int Width() { return right - left; }
        public int Height() { return bottom - top; }
        public int Square() { return Width() * Height(); }

        public SPDRect Set(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
            return this;
        }

        public SPDRect Set(SPDRect rect)
        {
            return Set(rect.left, rect.top, rect.right, rect.bottom);
        }

        public SPDRect SetPos(int x, int y)
        {
            return Set(x, y, x + (right - left), y + (bottom - top));
        }

        public SPDRect Shift(int x, int y)
        {
            return Set(left + x, top + y, right + x, bottom + y);
        }

        public SPDRect Resize(int w, int h)
        {
            return Set(left, top, left + w, top + h);
        }

        public bool IsEmpty()
        {
            return right <= left || bottom <= top;
        }

        public SPDRect SetEmpty()
        {
            left = right = top = bottom = 0;
            return this;
        }

        public SPDRect Intersect(SPDRect other)
        {
            var result = new SPDRect();
            result.left = Math.Max(left, other.left);
            result.right = Math.Min(right, other.right);
            result.top = Math.Max(top, other.top);
            result.bottom = Math.Min(bottom, other.bottom);
            return result;
        }

        public SPDRect Union(SPDRect other)
        {
            var result = new SPDRect();
            result.left = Math.Min(left, other.left);
            result.right = Math.Max(right, other.right);
            result.top = Math.Min(top, other.top);
            result.bottom = Math.Max(bottom, other.bottom);
            return result;
        }

        public SPDRect Union(int x, int y)
        {
            if (IsEmpty())
                return Set(x, y, x + 1, y + 1);

            if (x < left)
                left = x;
            else if (x >= right)
                right = x + 1;

            if (y < top)
                top = y;
            else if (y >= bottom)
                bottom = y + 1;
            return this;
        }

        public SPDRect Union(SPDPoint p) { return Union(p.x, p.y); }

        public bool Inside(SPDPoint p)
        {
            return p.x >= left && p.x < right && p.y >= top && p.y < bottom;
        }

        public SPDPoint Center()
        {
            return new SPDPoint(
                (left + right) / 2 + ((right - left) % 2 == 1 ? SPDRandom.Int(2) : 0),
                (top + bottom) / 2 + ((bottom - top) % 2 == 1 ? SPDRandom.Int(2) : 0)
                );
        }

        public IEnumerable<SPDPoint> GetPoints()
        {
            for (var i = left; i <= right; i++)
            {
                for (var j = top; j <= bottom; j++)
                    yield return new SPDPoint(i, j);
            }
        }
    }
}
