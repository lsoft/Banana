using System;
using System.Collections.Generic;

namespace Banana.Common.Others
{
    public static class EnumerableHelper
    {
        public static IEnumerable<ZipEntry<T1, T2>> LeftRightZip<T1, T2>(
            this IEnumerable<T1> collection1,
            IEnumerable<T2> collection2
            )
        {
            if (collection1 == null)
            {
                throw new ArgumentNullException("collection1");
            }
            if (collection2 == null)
            {
                throw new ArgumentNullException("collection2");
            }

            var index = 0;
            using (var enumerator1 = collection1.GetEnumerator())
            using (var enumerator2 = collection2.GetEnumerator())
            {
                while (true)
                {
                    var i = false;

                    T1 t1;

                    if (enumerator1.MoveNext())
                    {
                        t1 = enumerator1.Current;
                        i = true;
                    }
                    else
                    {
                        t1 = default(T1);
                    }

                    T2 t2;

                    if (enumerator2.MoveNext())
                    {
                        t2 = enumerator2.Current;
                        i = true;
                    }
                    else
                    {
                        t2 = default(T2);
                    }

                    if (!i)
                    {
                        break;
                    }

                    yield
                        return new ZipEntry<T1, T2>(
                            index,
                            t1,
                            t2
                            );

                    index++;
                }
            }
        }

        public static IEnumerable<ZipEntry<T1, T2>> LeftZip<T1, T2>(
            this IEnumerable<T1> collection1,
            IEnumerable<T2> collection2
            )
        {
            if (collection1 == null)
            {
                throw new ArgumentNullException("collection1");
            }
            if (collection2 == null)
            {
                throw new ArgumentNullException("collection2");
            }

            var index = 0;
            using (var enumerator1 = collection1.GetEnumerator())
            using (var enumerator2 = collection2.GetEnumerator())
            {
                while (enumerator1.MoveNext())
                {
                    T2 t2;

                    if (enumerator2.MoveNext())
                    {
                        t2 = enumerator2.Current;
                    }
                    else
                    {
                        t2 = default (T2);
                    }

                    yield
                        return new ZipEntry<T1, T2>(
                            index,
                            enumerator1.Current,
                            t2
                            );

                    index++;
                }
            }
        }

        public static IEnumerable<ZipEntry<T1, T2>> Zip<T1, T2>(
            this IEnumerable<T1> collection1,
            IEnumerable<T2> collection2
            )
        {
            if (collection1 == null)
            {
                throw new ArgumentNullException("collection1");
            }
            if (collection2 == null)
            {
                throw new ArgumentNullException("collection2");
            }

            var index = 0;
            using (var enumerator1 = collection1.GetEnumerator())
            using (var enumerator2 = collection2.GetEnumerator())
            {
                while (true)
                {
                    var hasNext1 = enumerator1.MoveNext();
                    var hasNext2 = enumerator2.MoveNext();

                    if (hasNext1 != hasNext2)
                    {
                        throw new InvalidOperationException("One of the collections ran out of values before the other");
                    }

                    if (!hasNext1)
                    {
                        break;
                    }

                    yield
                        return new ZipEntry<T1, T2>(
                            index,
                            enumerator1.Current,
                            enumerator2.Current
                            );

                    index++;
                }
            }
        }

        public sealed class ZipEntry<T1, T2>
        {
            public int Index
            {
                get;
                private set;
            }

            public T1 Value1
            {
                get;
                private set;
            }

            public T2 Value2
            {
                get;
                private set;
            }

            public ZipEntry(int index, T1 value1, T2 value2)
            {
                Index = index;
                Value1 = value1;
                Value2 = value2;
            }
        }

    }
}
