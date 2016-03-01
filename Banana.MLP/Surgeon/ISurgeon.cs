using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banana.MLP.Function;
using Banana.MLP.Layer;
using Banana.MLP.MLP;

namespace Banana.MLP.Surgeon
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
