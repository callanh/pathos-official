using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathos
{
    class SPDPoint
    {
        /// <summary>
        /// Point algorithm used by this module.
        /// </summary>
        public int x;
        public int y;

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

        public SPDPoint Set(int x, int y)
        {
            this.x = x;
            this.y = y;
            return this;
        }

        public SPDPoint Set(SPDPoint p)
        {
            x = p.x;
            y = p.y;
            return this;
        }

        public SPDPoint Scale(float f)
        {
            int newx = Convert.ToInt32(this.x * f);
            int newy = Convert.ToInt32(this.y * f);
            this.x = newx;
            this.y = newy;
            return this;
        }

        public SPDPoint Offset(int dx, int dy)
        {
            x += dx;
            y += dy;
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
