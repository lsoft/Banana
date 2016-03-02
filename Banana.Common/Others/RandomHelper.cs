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
        public static int GetRandomInt()
        {
            var g = Guid.NewGuid();
            var preg = g.ToString().Substring(0, 8);
            var seed = int.Parse(preg, NumberStyles.HexNumber);

            var now = DateTime.Now;
            var xormask = now.DayOfYear * 24 * 60 * 60 + (int)now.TimeOfDay.TotalSeconds;

            seed ^= xormask;

            return
                seed;
        }
    }
}
