using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Banana.Common.Others
{
    public static class KahanAlgorithm
    {
        public class Accumulator
        {
            private float _sum;
            private float _c;

            public float Sum
            {
                get
                {
                    return
                        _sum;
                }
            }

            public Accumulator(
                )
            {
                this._sum = 0f;
                this._c = 0f;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Add(
                float dataItem
                )
            {
                var y = dataItem - this._c;
                var t = this._sum + y;
                this._c = (t - this._sum) - y;
                this._sum = t;
            }

            public static implicit operator float(Accumulator acc)
            {
                return
                    acc._sum;
            }

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float PreciseSum(
            int dataCount,
            Func<int, float> floatProvider
            )
        {
            if (floatProvider == null)
            {
                throw new ArgumentNullException("floatProvider");
            }

            if (dataCount == 0)
            {
                return 0f;
            }

            var tempArray = new float[dataCount];
            for (var index = 0; index < dataCount; index++)
            {
                tempArray[index] = floatProvider(index);
            }

            return
                tempArray.PreciseSum();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float PreciseSum(
            this float[] data
            )
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            if (data.Length == 0)
            {
                return 0f;
            }

            var acc = new KahanAlgorithm.Accumulator();

            for (var cc = 0; cc < data.Length; cc++)
            {
                acc.Add(data[cc]);
            }

            return
                acc;

        }

    }
}
