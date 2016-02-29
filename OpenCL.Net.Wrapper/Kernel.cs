using System;
using System.Linq;
using OpenCL.Net.Wrapper.Mem;
using OpenCL.Net.Wrapper.Mem.Img;

namespace OpenCL.Net.Wrapper
{
    public class Kernel : IDisposable
    {
        private readonly CommandQueue _commandQueue;
        private readonly Program _program;
        private readonly OpenCL.Net.Kernel _kernel;

        private bool _disposed = false;

        public Kernel(
            CommandQueue commandQueue,
            Program program,
            string kernelName
            )
        {
            if (kernelName == null)
            {
                throw new ArgumentNullException("kernelName");
            }

            _commandQueue = commandQueue;
            _program = program;

            ErrorCode errorCode;
            _kernel = Cl.CreateKernel(_program, kernelName, out errorCode);

            if (errorCode != ErrorCode.Success)
            {
                throw new OpenCLException(
                    string.Format(
                        "CreateKernel fails, error code is: {0}",
                        errorCode
                        )
                    );
            }
        }

        public Kernel SetKernelArgLocalMem(uint argId, int size)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            var error = Cl.SetKernelArg(_kernel, argId, new IntPtr(size), null);

            if (error != ErrorCode.Success)
            {
                throw new OpenCLException(string.Format("Unable to run Cl.SetKernelArgMem: {0}!", error));
            }

            return this;
        }

        public Kernel SetKernelArgLocalMem(uint argId, uint size)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            var error = Cl.SetKernelArg(_kernel, argId, new IntPtr((int)size), null);

            if (error != ErrorCode.Success)
            {
                throw new OpenCLException(string.Format("Unable to run Cl.SetKernelArgMem: {0}!", error));
            }

            return this;
        }

        public Kernel SetKernelArgMem<T>(uint argId, Mem.Data.Mem<T> mem)
            where T : struct
        {
            if (mem == null)
            {
                throw new ArgumentNullException("mem");
            }

            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            var error = Cl.SetKernelArg(_kernel, argId, mem.GetMem());

            if (error != ErrorCode.Success)
            {
                throw new OpenCLException(string.Format("Unable to run Cl.SetKernelArgMem: {0}!", error));
            }

            return this;
        }

        public Kernel SetKernelArgImg(uint argId, IntensityFloatImg img)
        {
            if (img == null)
            {
                throw new ArgumentNullException("img");
            }

            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            var error = Cl.SetKernelArg(_kernel, argId, img.GetMem());

            if (error != ErrorCode.Success)
            {
                throw new OpenCLException(string.Format("Unable to run Cl.SetKernelArgMem: {0}!", error));
            }

            return this;
        }

        public Kernel SetKernelArg(uint argId, int size, object data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            var error = Cl.SetKernelArg(_kernel, argId, new IntPtr(size), data);
            
            if (error != ErrorCode.Success)
            {
                throw new OpenCLException(string.Format("Unable to run Cl.SetKernelArg: {0}!", error));
            }

            return this;
        }

        /// <summary>
        /// without local work sizes
        /// </summary>
        public void EnqueueNDRangeKernel(params ulong[] sizes)
        {
            if (sizes.Any(j => j == 0))
            {
                throw new ArgumentOutOfRangeException("sizes");
            }

            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            Event clevent;

            var error = Cl.EnqueueNDRangeKernel(
                _commandQueue,
                _kernel,
                (uint)sizes.Length,
                null,
                sizes.ToList().Select(size => new IntPtr((long)size)).ToArray(),
                null,
                0,
                null,
                out clevent);

            if (error != ErrorCode.Success)
            {
                throw new OpenCLException("EnqueueNDRangeKernel failed:" + error);
            }

            clevent.Dispose();
        }

        /// <summary>
        /// without local work sizes
        /// </summary>
        public void EnqueueNDRangeKernel(params int[] sizes)
        {
            if (sizes.Any(j => j <= 0))
            {
                throw new ArgumentOutOfRangeException("sizes");
            }

            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            Event clevent;

            var error = Cl.EnqueueNDRangeKernel(
                _commandQueue,
                _kernel,
                (uint)sizes.Length,
                null,
                sizes.ToList().Select(size => new IntPtr(size)).ToArray(),
                null,
                0,
                null,
                out clevent);

            clevent.Dispose();

            if (error != ErrorCode.Success)
            {
                throw new OpenCLException("EnqueueNDRangeKernel failed:" + error);
            }
        }

        /// <summary>
        /// with local work sizes
        /// </summary>
        public void EnqueueNDRangeKernel(ulong[] globalSizes, ulong[] localSizes)
        {
            if (globalSizes.Length != localSizes.Length)
            {
                throw new OpenCLException("globalSizes.Length != localSizes.Length");
            }
            if (globalSizes.Any(j => j == 0))
            {
                throw new ArgumentOutOfRangeException("globalSizes");
            }
            if (localSizes.Any(j => j == 0))
            {
                throw new ArgumentOutOfRangeException("localSizes");
            }

            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            Event clevent;

            var error = Cl.EnqueueNDRangeKernel(
                _commandQueue,
                _kernel,
                (uint)globalSizes.Length,
                null,
                globalSizes.ToList().Select(size => new IntPtr((long)size)).ToArray(),
                localSizes.ToList().Select(size => new IntPtr((long)size)).ToArray(),
                0,
                null,
                out clevent);

            clevent.Dispose();

            if (error != ErrorCode.Success)
            {
                throw new OpenCLException("EnqueueNDRangeKernel failed:" + error);
            }
        }

        /// <summary>
        /// with local work sizes
        /// </summary>
        public void EnqueueNDRangeKernel(uint[] globalSizes, uint[] localSizes)
        {
            if (globalSizes.Length != localSizes.Length)
            {
                throw new OpenCLException("globalSizes.Length != localSizes.Length");
            }
            if (globalSizes.Any(j => j == 0))
            {
                throw new ArgumentOutOfRangeException("globalSizes");
            }
            if (localSizes.Any(j => j == 0))
            {
                throw new ArgumentOutOfRangeException("localSizes");
            }

            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            Event clevent;

            var error = Cl.EnqueueNDRangeKernel(
                _commandQueue,
                _kernel,
                (uint)globalSizes.Length,
                null,
                globalSizes.ToList().Select(size => new IntPtr((int)size)).ToArray(),
                localSizes.ToList().Select(size => new IntPtr((int)size)).ToArray(),
                0,
                null,
                out clevent);

            clevent.Dispose();

            if (error != ErrorCode.Success)
            {
                throw new OpenCLException("EnqueueNDRangeKernel failed:" + error);
            }
        }

        /// <summary>
        /// with local work sizes
        /// </summary>
        public void EnqueueNDRangeKernel(int[] globalSizes, int[] localSizes)
        {
            if (globalSizes.Length != localSizes.Length)
            {
                throw new OpenCLException("globalSizes.Length != localSizes.Length");
            }

            if (globalSizes.Any(j => j <= 0))
            {
                throw new ArgumentOutOfRangeException("globalSizes");
            }

            if (localSizes.Any(j => j <= 0))
            {
                throw new ArgumentOutOfRangeException("localSizes");
            }

            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            Event clevent;
            
            var error = Cl.EnqueueNDRangeKernel(
                _commandQueue,
                _kernel,
                (uint) globalSizes.Length,
                null,
                globalSizes.ToList().Select(size => new IntPtr(size)).ToArray(),
                localSizes.ToList().Select(size => new IntPtr(size)).ToArray(),
                0,
                null,
                out clevent);

            clevent.Dispose();

            if (error != ErrorCode.Success)
            {
                throw new OpenCLException("EnqueueNDRangeKernel failed:" + error);
            }
        }


        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                Cl.ReleaseKernel(_kernel);
                Cl.ReleaseProgram(_program);
            }
        }
    }
}
