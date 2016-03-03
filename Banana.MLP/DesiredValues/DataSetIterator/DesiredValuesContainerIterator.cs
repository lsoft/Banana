using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Banana.Data.Item;
using Banana.Data.Set;
using Banana.Exception;

namespace Banana.MLP.DesiredValues.DataSetIterator
{
    public class DesiredValuesContainerIterator : IDataSetIterator
    {
        private readonly int _cacheSize;
        private readonly IDesiredValuesContainer _desiredValuesContainer;
        private readonly IDataSetIterator _iterator;

        private readonly ManualResetEventSlim _abortEvent = new ManualResetEventSlim(false);

        private ConcurrentQueue<IDataItem> _bgQueue;
        private ConcurrentQueue<IDataItem> _workQueue;

        private volatile Task _task;

        private volatile bool _disposed = false;

        public IDataItem Current
        {
            get;
            private set;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public int Count
        {
            get
            {
                return
                    _iterator.Count;
            }
        }

        public DesiredValuesContainerIterator(
            int cacheSize,
            IDesiredValuesContainer desiredValuesContainer,
            IDataSetIterator iterator
            )
        {
            if (desiredValuesContainer == null)
            {
                throw new ArgumentNullException("desiredValuesContainer");
            }
            if (iterator == null)
            {
                throw new ArgumentNullException("iterator");
            }
            if (cacheSize < 1)
            {
                throw new ArgumentException("cacheSize < 1");
            }

            throw new InvalidOperationException("Not tested! Please test extensively before use.");

            _cacheSize = cacheSize;
            _desiredValuesContainer = desiredValuesContainer;
            _iterator = iterator;

            Reset();
        }

        public bool MoveNext()
        {
            IDataItem newItem;
            if (!_workQueue.TryDequeue(out newItem))
            {
                WaitForWorkStopped();

                _workQueue = _bgQueue;

                StartWork();

                if (_bgQueue == null || !_workQueue.TryDequeue(out newItem))
                {
                    throw new BananaException(
                        "Background thread is also empty.",
                        BananaErrorEnum.InvalidOperation
                        );
                }
            }

            var result = newItem != null; //если null - коллекция закончилась

            this.Current = newItem;
            
            _desiredValuesContainer.MoveNext();

            return result;
        }

        public void Reset()
        {
            WaitForWorkStopped();

            _workQueue = new ConcurrentQueue<IDataItem>();

            _iterator.Reset();
            _desiredValuesContainer.Reset();

            StartWork();
        }


        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                StopWork();

                _abortEvent.Dispose();

                _iterator.Dispose();
                _desiredValuesContainer.Reset();
            }
        }

        private void StartWork()
        {
            if (_task != null)
            {
                throw new BananaException(
                    "The task is alive",
                    BananaErrorEnum.InvalidOperation);
            }

            _task = new Task(DoWork);
            _task.Start();
        }

        private void StopWork()
        {
            _abortEvent.Set();

            WaitForWorkStopped();
        }

        private void WaitForWorkStopped()
        {
            if (_task != null)
            {
                _task.Wait();
                _task.Dispose();
                _task = null;
            }
        }

        private void DoWork()
        {
            _bgQueue = new ConcurrentQueue<IDataItem>();

            var index = 0;

            while (index++ < _cacheSize && !_abortEvent.Wait(0))
            {
                if (_iterator.MoveNext())
                {
                    var i = _iterator.Current;

                    if (i == null)
                    {
                        throw new BananaException(
                            "Invalid arrived item",
                            BananaErrorEnum.DataError
                            );
                    }

                    _bgQueue.Enqueue(i);
                    _desiredValuesContainer.SetValues(i.Output);
                }
                else
                {
                    //no more data
                    _bgQueue.Enqueue(null);

                    _abortEvent.Set();
                }
            }
        }
    }
}
