

using System;

namespace OpenCL.Net.Wrapper.Mem.Data
{
    public class MemDouble : Mem<double>
    {
        internal MemDouble(
            Action<Guid> disposeExternalAction, 
            CommandQueue commandQueue,
            Context context,
            ulong arrayLength,
            MemFlags flags)
            : base(disposeExternalAction, commandQueue, context, arrayLength, 8, flags)
        {
        }

    }
}
