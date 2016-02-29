using System;

namespace OpenCL.Net.Wrapper.Mem.Data
{
    public class MemByte : Mem<byte>
    {
        internal MemByte(
            Action<Guid> disposeExternalAction,
            CommandQueue commandQueue,
            Context context,
            ulong arrayLength,
            MemFlags flags)
            : base(disposeExternalAction, commandQueue, context, arrayLength, 1, flags)
        {
        }
    }
}