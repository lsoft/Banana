using System;

namespace OpenCL.Net.Wrapper.Mem.Img
{
    public class IntensityFloatImg
        : FloatImg
    {
        internal IntensityFloatImg(
            Action<Guid> disposeExternalAction, 
            CommandQueue commandQueue, 
            Context context, 
            uint width, 
            uint height, 
            MemFlags flags)
            : base(disposeExternalAction, commandQueue, context, width, height, 1, ChannelOrder.Intensity, flags)
        {
        }
    }
}