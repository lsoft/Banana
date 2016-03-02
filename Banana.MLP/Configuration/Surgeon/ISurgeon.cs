using Banana.MLP.Configuration.Layer;
using Banana.MLP.Configuration.MLP;

namespace Banana.MLP.Configuration.Surgeon
{
    public interface ISurgeon
    {
        IMLPConfiguration CutLastLayer(
            IMLPConfiguration mlpConfiguration
            );

        IMLPConfiguration AutoencoderCutTail(
            IMLPConfiguration mlpConfiguration
            );

        IMLPConfiguration AutoencoderCutHead(
            IMLPConfiguration mlpConfiguration
            );

        IMLPConfiguration AddLayer(
            IMLPConfiguration mlpConfiguration,
            ILayerConfiguration layerConfiguration
            );

    }
}
