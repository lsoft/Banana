﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using OpenCL.Net.Extensions;
using OpenCL.Net.Wrapper.DeviceChooser;
using OpenCL.Net.Wrapper.Mem;
using OpenCL.Net.Wrapper.Mem.Data;
using OpenCL.Net.Wrapper.Mem.Img;
using OpenCL.Net.Wrapper.Properties;

namespace OpenCL.Net.Wrapper
{
    public sealed class CLProvider : IDisposable
    {
        private const int InfoBufferSize = 200000;

        private readonly Device _device;
        private readonly Context _context;
        private readonly CommandQueue _commandQueue;

        private readonly Dictionary<Guid, IMemWrapper> _mems;
        private readonly List<Kernel> _kernels;

        public Device ChoosedDevice
        {
            get
            {
                return
                    _device;
            }
        }

        public DeviceType ChoosedDeviceType
        {
            get;
            private set;
        }

        public CLParameters Parameters
        {
            get;
            private set;
        }

        private volatile bool _disposed = false;

        public CLProvider(
            IDeviceChooser deviceChooser,
            bool silentStart)
        {
            if (deviceChooser == null)
            {
                throw new ArgumentNullException("deviceChooser");
            }
            
            //ищем подходящее устройство opencl
            DeviceType choosedDeviceType;
            deviceChooser.ChooseDevice(out choosedDeviceType, out _device);

            this.ChoosedDeviceType = choosedDeviceType;

            this.Parameters = new CLParameters(this.ChoosedDevice);

            if (!silentStart)
            {
                //выводим его параметры
                this.Parameters.DumpToConsole();
            }

            //создаем контекст вычислений
            _context = CreateContext();

            //создаем очередь команд
            _commandQueue = CreateCommandQueue();

            //создаем пустые структуры
            _mems =new Dictionary<Guid, IMemWrapper>();
            _kernels = new List<Kernel>();
        }

        public CLProvider(
            bool silentStart = true)
             : this(new IntelCPUDeviceChooser(), silentStart)
        {
        }

        /// <summary>
        /// загружаем программу и параметры
        /// </summary>
        /// <param name="source">Текст кернела (или кернелов)</param>
        /// <param name="kernelName">Имя кернела</param>
        public Kernel CreateKernel(
            string source,
            string kernelName)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
            
            var fullTextStringBuilder = new StringBuilder();
            fullTextStringBuilder.Append(Resources.Reduction);
            fullTextStringBuilder.Append(Resources.KahanAlgorithm);
            fullTextStringBuilder.Append(Resources.KahanAlgorithm4);
            fullTextStringBuilder.Append(Resources.KahanAlgorithm16);
            fullTextStringBuilder.Append(source);

            var fullText = fullTextStringBuilder.ToString();

            ErrorCode error;

            var program = Cl.CreateProgramWithSource(_context, 1, new[] { fullText }, new IntPtr[] { (IntPtr)fullText.Length }, out error);
            if (error != ErrorCode.Success)
            {
                throw new OpenCLException(string.Format("Unable to run Cl.CreateProgramWithSource for program: {0}!", error));
            }

            error = Cl.BuildProgram(
                program,
                1,
                new[] { _device },
                string.Empty,
                //"-cl-opt-disable",
                //"-cl-opt-disable -cl-single-precision-constant -cl-fast-relaxed-math", 
                //"-cl-denorms-are-zero",
                //"-cl-single-precision-constant",
                //"-cl-mad-enable",
                null,
                IntPtr.Zero
                );

            if (error != ErrorCode.Success)
            {
                using (var infoBuffer = new InfoBuffer(new IntPtr(InfoBufferSize)))
                {
                    IntPtr retSize;
                    Cl.GetProgramBuildInfo(program, _device, ProgramBuildInfo.Log, new IntPtr(InfoBufferSize), infoBuffer, out retSize);

                    throw new OpenCLException(
                        string.Format(
                            "Error building program {0}.\r\n{1}",
                            infoBuffer,
                            fullText)
                        );
                }
            }

            var buildStatus = Cl.GetProgramBuildInfo(program, _device, ProgramBuildInfo.Status, out error).CastTo<BuildStatus>();

            if (buildStatus != BuildStatus.Success)
            {
                throw new OpenCLException(
                    string.Format(
                        "GetProgramBuildInfo returned {0} for program!\r\n{1}",
                        buildStatus,
                        fullText
                        ));
            }

            var k = new Kernel(
                _commandQueue,
                program,
                kernelName);

            _kernels.Add(k);

            return k;
        }

        public MemUint CreateUintMem(
            long arrayLength,
            MemFlags flags)
        {
            if (arrayLength <= 0L)
            {
                throw new ArgumentOutOfRangeException("arrayLength");
            }

            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            var memi = new MemUint(
                DisposeMemFunc,
                _commandQueue,
                _context,
                (ulong)arrayLength,
                flags);

            this._mems.Add(memi.MemGuid, memi);

            return memi;
        }

        public MemUint CreateUintMem(
            ulong arrayLength,
            MemFlags flags)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            var memi = new MemUint(
                DisposeMemFunc,
                _commandQueue,
                _context,
                arrayLength,
                flags);

            this._mems.Add(memi.MemGuid, memi);

            return memi;
        }

        public MemInt CreateIntMem(
            long arrayLength,
            MemFlags flags)
        {
            if (arrayLength <= 0L)
            {
                throw new ArgumentOutOfRangeException("arrayLength");
            }

            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            var memi = new MemInt(
                DisposeMemFunc,
                _commandQueue,
                _context,
                (ulong)arrayLength,
                flags);

            this._mems.Add(memi.MemGuid, memi);

            return memi;
        }


        public MemInt CreateIntMem(
            ulong arrayLength,
            MemFlags flags)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            var memi = new MemInt(
                DisposeMemFunc,
                _commandQueue,
                _context,
                arrayLength,
                flags);

            this._mems.Add(memi.MemGuid, memi);

            return memi;
        }

        public MemHalf CreateHalfMem(
            long arrayLength,
            MemFlags flags)
        {
            if (arrayLength <= 0L)
            {
                throw new ArgumentOutOfRangeException("arrayLength");
            }

            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            var memh = new MemHalf(
                DisposeMemFunc,
                _commandQueue,
                _context,
                (ulong)arrayLength,
                flags);

            this._mems.Add(memh.MemGuid, memh);

            return memh;
        }

        public MemHalf CreateHalfMem(
            ulong arrayLength,
            MemFlags flags)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            var memh = new MemHalf(
                DisposeMemFunc,
                _commandQueue,
                _context,
                arrayLength,
                flags);

            this._mems.Add(memh.MemGuid, memh);

            return memh;
        }


        public MemByte CreateByteMem(
            ulong arrayLength,
            MemFlags flags)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            var memb = new MemByte(
                DisposeMemFunc,
                _commandQueue,
                _context,
                arrayLength,
                flags);

            this._mems.Add(memb.MemGuid, memb);

            return memb;
        }


        public MemFloat CreateFloatMem(
            long arrayLength,
            MemFlags flags)
        {
            if (arrayLength <= 0L)
            {
                throw new ArgumentOutOfRangeException("arrayLength");
            }

            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            var memf = new MemFloat(
                DisposeMemFunc,
                _commandQueue,
                _context,
                (ulong)arrayLength,
                flags);

            this._mems.Add(memf.MemGuid, memf);

            return memf;
        }

        public MemFloat CreateFloatMem(
            ulong arrayLength,
            MemFlags flags)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            var memf = new MemFloat(
                DisposeMemFunc,
                _commandQueue,
                _context,
                arrayLength,
                flags);

            this._mems.Add(memf.MemGuid, memf);

            return memf;
        }

        public MemDouble CreateDoubleMem(
            ulong arrayLength,
            MemFlags flags)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            var memd = new MemDouble(
                DisposeMemFunc,
                _commandQueue,
                _context,
                arrayLength,
                flags);

            this._mems.Add(memd.MemGuid, memd);

            return memd;
        }

        public IntensityFloatImg CreateImg(
            uint width,
            uint height,
            MemFlags flags)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            var memf = new IntensityFloatImg(
                DisposeMemFunc,
                _commandQueue,
                _context,
                width,
                height,
                flags);

            this._mems.Add(memf.MemGuid, memf);

            return memf;
        }

        public void QueueFinish()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            _commandQueue.Finish();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                foreach (var m in _mems)
                {
                    m.Value.Dispose();
                }

                foreach (var k in _kernels)
                {
                    k.Dispose();
                }

                Cl.ReleaseCommandQueue(_commandQueue);
                Cl.ReleaseContext(_context);
            }
        }

        #region private code

        private void DisposeMemFunc(Guid memGuid)
        {
            if (_disposed)
            {
                return;
            }

            var removed = _mems.Remove(memGuid);

            if (!removed)
            {
                throw new OpenCLException(
                    string.Format(
                        "Буфер с ГУИДом {0} не найден",
                        memGuid
                        ));
            }
        }

        private CommandQueue CreateCommandQueue()
        {
            ErrorCode error;

            var commandQueue = Cl.CreateCommandQueue(_context, _device, (CommandQueueProperties)0, out error);
            if (error != ErrorCode.Success)
            {
                throw new OpenCLException(string.Format("Unable to CreateCommandQueue: {0}!", error));
            }

            return commandQueue;
        }

        private Context CreateContext()
        {
            ErrorCode error;

            var context = Cl.CreateContext(null, 1, new[] { _device }, null, IntPtr.Zero, out error);

            if (error != ErrorCode.Success)
            {
                throw new OpenCLException(string.Format("Unable to retrieve an OpenCL Context, error was: {0}!", error));
            }

            return
                context;
        }

        #endregion
    }
}
