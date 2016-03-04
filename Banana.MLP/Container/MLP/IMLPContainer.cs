using Banana.MLP.Configuration.MLP;
using Banana.MLP.Container.Layer.CSharp;

namespace Banana.MLP.Container.MLP
{
    public interface IMLPContainer<T>
        where T : ILayerContainer
    {
        IMLPConfiguration Configuration
        {
            get;
        }

        T[] Layers
        {
            get;
        }
    }
}