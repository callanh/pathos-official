using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathos
{
    static class SPDRandom
    {
        public static float Float()
        {
            return (float)RandomSupport.NextPercentile();
        }

        public static float Float(float max)
        {
            return Float() * max;
        }

        public static float Float(float min, float max)
        {
            return min + Float(max - min);
        }

        public static float NormalFloat(float min, float max)
        {
            return min + ((Float(max - min) + Float(max - min)) / 2f);
        }

        public static int Int(int max)
        {
            return max > 0 ? RandomSupport.NextNumber(max) : 0;
        }

        public static int Int(int min, int max)
        {
            return min + Int(max - min);
        }

        public static int IntRange(int min, int max)
        {
            return min + Int(max - min + 1);
        }

        public static int NormalIntRange(int min, int max)
        {
            return min + (int)((Float() + Float()) * (max - min + 1) / 2f);
        }

        public static int Chances(float[] chances)
        {
            int length = chances.Length;
            float sum = 0;
            for (int i = 0; i < length; i++)
            {
                sum += chances[i];
            }

            float value = Float() * sum;
            float sum2 = 0;
            for (int i = 0; i < length; i++)
            {
                sum2 = sum2 + chances[i];
                if (value <= sum2)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}