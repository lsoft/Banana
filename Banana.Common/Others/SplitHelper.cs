using System;
using System.Collections.Generic;
using System.Linq;

namespace Banana.Common.Others
{
    public static class SplitHelper
    {
        public static IEnumerable<List<T>> LazySplit<T>(
            this IEnumerator<T> list,
            int splitCount
            )
        {
            if (splitCount <= 0)
            {
                throw new ArgumentException("splitCount <= 0");
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
