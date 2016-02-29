using System;

namespace OpenCL.Net.Wrapper.Mem.Img
{
    public abstract class FloatImg
        : IMemWrapper
    {
        private readonly Action<Guid> _disposeExternalAction;
        private readonly CommandQueue _commandQueue;
        private readonly uint _width;
        private readonly uint _height;

        private readonly IMem _mem;

        private bool _disposed;

        public float[] Array;

        public Guid MemGuid
        {
            get;
            private set;
        }

        protected internal FloatImg(
            Action<Guid> disposeExternalAction,
            CommandQueue commandQueue,
            Context context,
            uint width,
            uint height,
            uint pixelSizeofInFloats,
            ChannelOrder channelOrder,
            MemFlags flags
            )
        {
            if (disposeExternalAction == null)
            {
                throw new ArgumentNullException("disposeExternalAction");
            }

            _disposeExternalAction = disposeExternalAction;
            _commandQueue = commandQueue;
            _width = width;
            _height = height;

            Array = new float[width * height * pixelSizeofInFloats];

            MemGuid = Guid.NewGuid();


            ErrorCode errorcodeRet;
            _mem = Cl.CreateImage2D(
                context,
                flags,
                new ImageFormat(
                    channelOrder,
                    ChannelType.Float),
                (IntPtr)width,
                (IntPtr)height,
                (IntPtr)0,
                Array,
                out errorcodeRet);
        }

        public IMem GetMem()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            return
                _mem;
        }

        public void Write(
            BlockModeEnum blockMode
            )
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            var blocking = blockMode == BlockModeEnum.Blocking ? Bool.True : Bool.False;

            var originPtr = new IntPtr[] { (IntPtr)0, (IntPtr)0, (IntPtr)0 };    //x, y, z
            var regionPtr = new IntPtr[] { (IntPtr)_width, (IntPtr)_height, (IntPtr)1 };    //x, y, z

            Event writeEvent;

            var error = Cl.EnqueueWriteImage(
                _commandQueue,
                _mem,
                blocking,
                originPtr,
                regionPtr,
                (IntPtr) 0,
                (IntPtr) 0,
                Array,
                0,
                null,
                out writeEvent
                );

            writeEvent.Dispose();

            if (error != ErrorCode.Success)
            {
                throw new OpenCLException(string.Format("EnqueueWriteImage failed: {0}!", error));
            }
        }

        public void Read(
            BlockModeEnum blockMode
            )
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            var blocking = blockMode == BlockModeEnum.Blocking ? Bool.True : Bool.False;

            var originPtr = new IntPtr[] { (IntPtr)0, (IntPtr)0, (IntPtr)0 };    //x, y, z
            var regionPtr = new IntPtr[] { (IntPtr)_width, (IntPtr)_height, (IntPtr)1 };    //x, y, z

            Event writeEvent;

            var error = Cl.EnqueueReadImage(
                _commandQueue,
                _mem, 
                blocking,
                originPtr,
                regionPtr,
                (IntPtr)0,
                (IntPtr)0,
                Array,
                0, 
                null, 
                out writeEvent
                );

            writeEvent.Dispose();

            if (error != ErrorCode.Success)
            {
                throw new OpenCLException(string.Format("EnqueueReadImage failed: {0}!", error));
            }
        }

        public void Dispose()
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