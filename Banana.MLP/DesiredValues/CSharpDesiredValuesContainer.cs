using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Banana.MLP.DesiredValues
{
    public class CSharpDesiredValuesContainer : ICSharpDesiredValuesContainer
    {
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

            throw new InvalidOperationException("Not tested! Please test extensively before use.");

        }

        public void SetValues(float[] desiredValues)
        {
            if (desiredValues == null)
            {
                throw new ArgumentNullException("desiredValues");
            }

            _queue.Enqueue(desiredValues);

            Refresh(true);
        }

        public bool MoveNext()
        {
            return 
                Refresh(false);
        }

        public void Reset()
        {
            _queue.Clear();
            DesiredOutput = null;
        }

        private bool Refresh(
            bool onlyIfEmpty
            )
        {
            var result = false;
            
            var allowedtoProceed = false;
            if (onlyIfEmpty)
            {
                if (DesiredOutput == null)
                {
                    allowedtoProceed = true;
                }
            }
            else
            {
                allowedtoProceed = true;
            }

            if (allowedtoProceed)
            {
                if(_queue.Count > 0)
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