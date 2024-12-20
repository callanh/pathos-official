using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathos
{
  class SPDPoint
  {
    public SPDPoint() { }
    public SPDPoint(int x, int y)
    {
      this.x = x;
      this.y = y;
    }
    public SPDPoint(SPDPoint p)
    {
      this.x = p.x;
      this.y = p.y;
    }

    /// <summary>
    /// Point algorithm used by this module.
    /// </summary>
    public int x;
    public int y;

    public SPDPoint Set(int x, int y)
    {
      this.x = x;
      this.y = y;
      return this;
    }
    public SPDPoint Offset(SPDPoint d)
    {
      x += d.x;
      y += d.y;
      return this;
    }
  }
}