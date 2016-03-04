using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Banana.Common
{
    public static class ForHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForBetween(
            this int fromInclusive,
            int toExclusive,
            Action<int> workAction
            )
        {
            if (workAction == null)
            {
                throw new ArgumentNullException("workAction");
            }

#if SINGLE_THREAD
            for (var cc = fromInclusive; cc < toExclusive; cc++)
            {
                workAction(cc);
            }
#else
            Parallel.For(fromInclusive, toExclusive, workAction);
#endif
        }
    }
}
