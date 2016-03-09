using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Banana.MLP.DesiredValues
{
    public class CSharpDesiredValuesContainer : ICSharpDesiredValuesContainer
    {
        private readonly object _locker = new object();

        private readonly Queue<float[]> _queue = new Queue<float[]>();

        public float[] DesiredOutput
        {
            get;
            private set;
        }


        public CSharpDesiredValuesContainer(
            )
        {
            DesiredOutput = null;
        }

        public void Enqueue(float[] desiredValues)
        {
            if (desiredValues == null)
            {
                throw new ArgumentNullException("desiredValues");
            }

            lock (_locker)
            {
                _queue.Enqueue(desiredValues);
            }
        }

        public bool MoveNext()
        {
            return 
                Refresh();
        }

        public void Reset()
        {
            lock (_locker)
            {
                _queue.Clear();
            }

            DesiredOutput = null;
        }

        private bool Refresh(
            )
        {
            var result = false;
            
            lock (_locker)
            {
                if (_queue.Count > 0)
                {
                    DesiredOutput = _queue.Dequeue();

                    result = true;
                }
            }

            return
                result;
        }
    }
}