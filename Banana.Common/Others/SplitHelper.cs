using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Banana.Common.Ambient;

namespace Banana.Common.Others
{
    public static class SplitHelper
    {
        private static int _logged = 0;

        public static IEnumerable<List<T>> LazySplit<T>(
            this IEnumerator<T> list,
            int splitCount
            )
        {
            if (splitCount <= 0)
            {
                throw new ArgumentException("splitCount <= 0");
            }

            if (splitCount == 1)
            {
                if (Interlocked.Exchange(ref _logged, 1) == 0)
                {
                    ConsoleAmbientContext.Console.WriteWarning("it's very inefficient to use SplitHelper.LazySplit with Split count = 1. Try do not use this function in case of split count = 1.");
                }
            }

            var result = new List<T>();

            var currentIndex = 0;
            while(list.MoveNext())
            {
                result.Add(list.Current);

                if (++currentIndex >= splitCount)
                {
                    yield return
                        result;

                    result = new List<T>();
                    currentIndex = 0;
                }
            }

            if (result.Count > 0)
            {
                yield return
                    result;
            }
        }
    }
}
