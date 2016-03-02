using Banana.MLP.Configuration.MLP;
using Banana.MLP.Container.Layer.CSharp;

namespace Banana.MLP.Container.MLP
{
    public interface IMLPContainer
    {
        IMLPConfiguration Configuration
        {
            get;
        }

        ILayerContainer[] Layers
        {
            get;
        }
    }
}