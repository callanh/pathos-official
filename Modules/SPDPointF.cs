using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathos
{
    class SPDPointF
    {
        public static readonly float PI = 3.1415926f;
        public static readonly float PI2 = PI * 2;
        public static readonly float G2R = PI / 180;

        public float x;
        public float y;

        public SPDPointF() { }
        public SPDPointF(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public SPDPointF(SPDPointF p)
        {
            this.x = p.x;
            this.y = p.y;
        }

        public SPDPointF(SPDPoint p)
        {
            this.x = p.x;
            this.y = p.y;
        }

        public SPDPointF Scale(float f)
        {
            this.x *= f;
            this.y *= f;
            return this;
        }

        public SPDPointF InvScale(float f)
        {
            this.x /= f;
            this.y /= f;
            return this;
        }

        public SPDPointF Set(float x, float y)
        {
            this.x = x;
            this.y = y;
            return this;
        }

        public SPDPointF Set(SPDPointF p)
        {
            this.x = p.x;
            this.y = p.y;
            return this;
        }

        public SPDPointF Set(float v)
        {
            this.x = v;
            this.y = v;
            return this;
        }

        public SPDPointF Polar(float a, float l)
        {
            this.x = l * (float)Math.Cos(a);
            this.y = l * (float)Math.Sin(a);
            return this;
        }

        public SPDPointF Offset(float dx, float dy)
        {
            x += dx;
            y += dy;
            return this;
        }

        public SPDPointF Offset(SPDPointF p)
        {
            x += p.x;
            y += p.y;
            return this;
        }

        public SPDPointF Negate()
        {
            x = -x;
            y = -y;
            return this;
        }

        public SPDPointF Normalize()
        {
            float l = Length();
            x /= l;
            y /= l;
            return this;
        }

        public SPDPoint Floor()
        {
            return new SPDPoint((int)x, (int)y);
        }

        public float Length()
        {
            return (float)Math.Sqrt(x * x + y * y);
        }

        public static SPDPointF Diff(SPDPointF a, SPDPointF b)
        {
            return new SPDPointF(a.x - b.x, a.y - b.y);
        }

        public static SPDPointF Inter(SPDPointF a, SPDPointF b, float d)
        {
            return new SPDPointF(a.x + (b.x - a.x) * d, a.y + (b.y - a.y) * d);
        }

        public static float Distance(SPDPointF a, SPDPointF b)
        {
            float dx = a.x - b.x;
            float dy = a.y - b.y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        public static float Angle(SPDPointF start, SPDPointF end)
        {
            return (float)Math.Atan2(end.y - start.y, end.x - start.x);
        }
    }
}
