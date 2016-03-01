using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Banana.MLP.Layer;

namespace Banana.MLP.MLP
{
    public interface IMLPConfiguration
    {
        string Name
        {
            get;
        }

        ILayerConfiguration[] Layers
        {
            get;
        }

        string GetLayerInformation();
    }
}
