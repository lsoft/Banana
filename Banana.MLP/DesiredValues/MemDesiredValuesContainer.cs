using System;
using System.Collections.Generic;
using Banana.Exception;
using OpenCL.Net;
using OpenCL.Net.Wrapper;
using OpenCL.Net.Wrapper.Mem;
using OpenCL.Net.Wrapper.Mem.Data;

namespace Banana.MLP.DesiredValues
{
    public class MemDesiredValuesContainer : IMemDesiredValuesContainer
    {
        private readonly CLProvider _clProvider;
        private readonly int _maxUnusedQueueSize;

        private readonly Queue<MemFloat> _queue = new Queue<MemFloat>();
        private readonly Queue<MemFloat> _unusedQueue = new Queue<MemFloat>();

        public MemFloat DesiredOutput
        {
            get;
            private set;
        }

        public MemDesiredValuesContainer(
            CLProvider clProvider,
            int maxUnusedQueueSize
            )
        {
            if (clProvider == null)
            {
                throw new ArgumentNullException("clProvider");
            }
            if (maxUnusedQueueSize <= 0)
            {
                throw new ArgumentException("maxUnusedQueueSize <= 0");
            }

            throw new InvalidOperationException("Not tested! Please test extensively before use.");

            _clProvider = clProvider;
            _maxUnusedQueueSize = maxUnusedQueueSize;
        }

        public void Enqueue(float[] desiredValues)
        {
            if (desiredValues == null)
            {
                throw new ArgumentNullException("desiredValues");
            }
            if (desiredValues.Length != DesiredOutput.Array.Length)
            {
                throw new InvalidOperationException("desiredValues.Length != DesiredOutput.Array.Length");
            }

            MemFloat newMem;

            if (_unusedQueue.Count > 0)
            {
                newMem = _unusedQueue.Dequeue();
            }
            else
            {
                newMem = _clProvider.CreateFloatMem(
                    desiredValues.Length,
                    MemFlags.CopyHostPtr | MemFlags.ReadOnly
                    );
            }

            if (newMem.Array.Length != desiredValues.Length)
            {
                throw new BananaException(
                    "Inconsistency detected",
                    BananaErrorEnum.DataError
                    );
            }

            desiredValues.CopyTo(newMem.Array, 0);

            newMem.Write(BlockModeEnum.NonBlocking);

            _queue.Enqueue(newMem);
        }

        public bool MoveNext()
        {
            return
                Refresh();
        }

        public void Reset()
        {
            var current = this.DesiredOutput;
            if (current != null)
            {
                current.Dispose();
            }

            foreach (var mem in _queue)
            {
                mem.Dispose();
            }

            foreach (var mem in _unusedQueue)
            {
                mem.Dispose();
            }

            this.DesiredOutput = null;

            _queue.Clear();
            _unusedQueue.Clear();
        }

        private bool Refresh(
            )
        {
            var result = false;
            
            if (_queue.Count > 0)
            {
                //убираем актуальный мем
                if (DesiredOutput != null)
                {
                    if (_unusedQueue.Count < _maxUnusedQueueSize)
                    {
                        _unusedQueue.Enqueue(DesiredOutput);
                    }
                    else
                    {
                        DesiredOutput.Dispose();
                    }

                    DesiredOutput = null;
                }

                //заменяем на новый мем
                DesiredOutput = _queue.Dequeue();

                result = true;
            }

            return
                result;
        }
    }
}