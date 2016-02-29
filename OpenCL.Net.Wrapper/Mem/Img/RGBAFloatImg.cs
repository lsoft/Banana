using System;

namespace OpenCL.Net.Wrapper.Mem.Img
{
    public class RGBAFloatImg
        : FloatImg
    {
        internal RGBAFloatImg(
            Action<Guid> disposeExternalAction,
            CommandQueue commandQueue,
            Context context,
            uint width,
            uint height,
            MemFlags flags)
            : base(disposeExternalAction, commandQueue, context, width, height, 4, ChannelOrder.RGBA, flags)
        {
        }
    }
}