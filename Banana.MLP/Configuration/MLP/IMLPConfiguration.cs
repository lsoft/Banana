using Banana.MLP.Configuration.Layer;

namespace Banana.MLP.Configuration.MLP
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

        string GetMLPScheme();
    }
}
