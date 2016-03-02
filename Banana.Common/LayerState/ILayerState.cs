using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banana.Common.LayerState
{
    /// <summary>
    /// состояние слоя после просчета
    /// </summary>
    public interface ILayerState : IEnumerable<float>
    {
        float[] NState
        {
            get;
        }
    }
}
