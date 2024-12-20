using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathos
{
  class SPDRect
  {
    public SPDRect(int left, int top, int right, int bottom)
    {
      this.left = left;
      this.top = top;
      this.right = right;
      this.bottom = bottom;
    }

    /// <summary>
    /// Rectangle algorithm used by this module.
    /// </summary>
    public int left;
    public int top;
    public int right;
    public int bottom;

    public int Width() => right - left;
    public int Height() => bottom - top;

    public SPDRect Set(int left, int top, int right, int bottom)
    {
      this.left = left;
      this.top = top;
      this.right = right;
      this.bottom = bottom;
      return this;
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
      return new SPDRect
      (
        left: Math.Max(left, other.left),
        right: Math.Min(right, other.right),
        top: Math.Max(top, other.top),
        bottom: Math.Min(bottom, other.bottom)
      );
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