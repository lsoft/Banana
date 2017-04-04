using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banana.Common.Others
{
    public static class RandomHelper
    {
        public static void Fill(
            this Random randomizer,
            float[] m,
            int maxValue,
            int shift
            )
        {
            for (var cc = 0; cc < m.Length; cc++)
            {
                m[cc] = randomizer.Next(maxValue) + shift;
            }
        }

        public static void Fill(
            this Random randomizer,
            float[] m,
            int maxValue,
            int shift,
            float scaler
            )
        {
            for (var cc = 0; cc < m.Length; cc++)
            {
                m[cc] = (randomizer.Next(maxValue) + shift) / scaler;
            }
        }

        public static void Fill(
            this Random randomizer,
            double[] m,
            int maxValue,
            int shift
            )
        {
            for (var cc = 0; cc < m.Length; cc++)
            {
                m[cc] = randomizer.Next(maxValue) + shift;
            }
        }

        public static void Fill(
            this Random randomizer,
            double[] m,
            int maxValue,
            int shift,
            double scaler
            )
        {
            for (var cc = 0; cc < m.Length; cc++)
            {
                m[cc] = (randomizer.Next(maxValue) + shift) / scaler;
            }
        }

        public static int GetPositiveRandomInt(int maxValue)
        {
            var g = Guid.NewGuid();
            var preg = g.ToString().Substring(0, 8);
            var seed = int.Parse(preg, NumberStyles.HexNumber);

            seed &= 0x7fffffff;

            seed = seed % maxValue;
            
            return
                seed;
        }

        public static int GetRandomInt(uint maxValueByAbsoluteValue)
        {
            if (maxValueByAbsoluteValue >= (uint.MaxValue >> 1))
            {
                throw new ArgumentOutOfRangeException("maxValueByAbsoluteValue");
            }

            var g = Guid.NewGuid();
            var preg = g.ToString().Substring(0, 8);
            var seed = uint.Parse(preg, NumberStyles.HexNumber);

            seed = seed % (maxValueByAbsoluteValue * 2);

            var value = (int)seed - (int)maxValueByAbsoluteValue;

            return
                value;
        }

        public static int GetRandomInt()
        {
            var g = Guid.NewGuid();
            var preg = g.ToString().Substring(0, 8);
            var seed = int.Parse(preg, NumberStyles.HexNumber);

            return
                seed;
        }
    }
}
