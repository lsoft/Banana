using System;

namespace OpenCL.Net.Wrapper.Mem.Data
{
    public class MemHalf : Mem<Half>
    {
        internal MemHalf(
            Action<Guid> disposeExternalAction,
            CommandQueue commandQueue,
            Context context,
            ulong arrayLength,
            MemFlags flags)
            : base(disposeExternalAction, commandQueue, context, arrayLength, 2, flags)
        {
        }
    }
}