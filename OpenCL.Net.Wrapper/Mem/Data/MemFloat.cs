

using System;

namespace OpenCL.Net.Wrapper.Mem.Data
{
    public class MemFloat : Mem<float>
    {
        internal MemFloat(
            Action<Guid> disposeExternalAction,
            CommandQueue commandQueue,
            Context context,
            ulong arrayLength,
            MemFlags flags)
            : base(disposeExternalAction, commandQueue, context, arrayLength, 4, flags)
        {
        }
    }
}
