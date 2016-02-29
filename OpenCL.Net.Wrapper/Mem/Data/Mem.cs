using System;
using OpenCL.Net.Extensions;

namespace OpenCL.Net.Wrapper.Mem.Data
{
    public abstract class Mem<T>
        : IMemWrapper
        where T : struct
    {
        private readonly Action<Guid> _disposeExternalAction;
        private readonly CommandQueue _commandQueue;
        private readonly IMem<T> _mem;

        private bool _disposed;

        public T[] Array;

        protected int _sizeOfT
        {
            get;
            private set;
        }

        public Guid MemGuid
        {
            get;
            private set;
        }

        protected internal Mem(
            Action<Guid> disposeExternalAction,
            CommandQueue commandQueue,
            Context context,
            ulong arrayLength,
            int sizeOfT,
            MemFlags flags
            )
        {
            if (disposeExternalAction == null)
            {
                throw new ArgumentNullException("disposeExternalAction");
            }
            
            _disposeExternalAction = disposeExternalAction;
            _commandQueue = commandQueue;
            _sizeOfT = sizeOfT;

            Array = new T[arrayLength];

            MemGuid = Guid.NewGuid();

            _mem = context.CreateBuffer(Array, flags);
        }

        public OpenCL.Net.IMem<T> GetMem()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            return
                _mem;
        }

        public void Write(BlockModeEnum blockMode)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            var blocking = blockMode == BlockModeEnum.Blocking ? Bool.True : Bool.False;

            Event writeEvent;

            var error = Cl.EnqueueWriteBuffer(_commandQueue, _mem, blocking, Array, 0, null, out writeEvent);

            writeEvent.Dispose();

            if (error != ErrorCode.Success)
            {
                throw new OpenCLException(string.Format("EnqueueWriteBuffer failed: {0}!", error));
            }
        }

        public void Read(BlockModeEnum blockMode)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            var blocking = blockMode == BlockModeEnum.Blocking ? Bool.True : Bool.False;

            Event writeEvent;

            var error = Cl.EnqueueReadBuffer(_commandQueue, _mem, blocking, Array, 0, null, out writeEvent);

            writeEvent.Dispose();

            if (error != ErrorCode.Success)
            {
                throw new OpenCLException(string.Format("EnqueueReadBuffer failed: {0}!", error));
            }
        }

        public virtual void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                this.Array = null;

                Cl.ReleaseMemObject(_mem);

                _disposeExternalAction(this.MemGuid);
            }
        }
    }
}