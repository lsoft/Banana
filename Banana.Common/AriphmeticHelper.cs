using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Banana.Common
{
    public static class AriphmeticHelper
    {
        public delegate void FloatDelegate(ref float a, float b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add(ref float a, float b)
        {
            a += b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Assign(ref float a, float b)
        {
            a = b;
        }
    }
}
