using OpenCL.Net.Wrapper.Mem.Data;

namespace Banana.MLP.DesiredValues
{
    public interface IMemDesiredValuesContainer : IDesiredValuesContainer
    {
        MemFloat DesiredOutput
        {
            get;
        }
    }
}