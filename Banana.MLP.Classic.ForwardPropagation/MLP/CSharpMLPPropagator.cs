using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banana.Data.Item;
using Banana.MLP.Classic.ForwardPropagation.Layer;
using Banana.MLP.Container.MLP;
using Banana.MLP.Propagator.Layer;
using Banana.MLP.Propagator.MLP;

namespace Banana.MLP.Classic.ForwardPropagation.MLP
{
    public class CSharpMLPPropagator : IMLPPropagator
    {
        public IMLPContainer MLPContainer
        {
            get;
            private set;
        }

        public ILayerPropagator[] ForwardPropagators
        {
            get;
            private set;
        }

        public CSharpMLPPropagator(
            IMLPContainer mlpContainer
            )
        {
            if (mlpContainer == null)
            {
                throw new ArgumentNullException("mlpContainer");
            }

            MLPContainer = mlpContainer;

            ForwardPropagators = new ILayerPropagator[mlpContainer.Layers.Length];

            for (var layerIndex = 1; layerIndex < mlpContainer.Layers.Length; layerIndex++)
            {
                var previousLayerContainer = mlpContainer.Layers[layerIndex - 1];
                var currentLayerContainer = mlpContainer.Layers[layerIndex];

                ForwardPropagators[layerIndex] = new CSharpLayerPropagator(
                    previousLayerContainer,
                    currentLayerContainer
                    );
            }
        }

        public void Propagate(
            IDataItem item
            )
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            var inputLayer = ForwardPropagators[0].LayerContainer;

            inputLayer.ReadInput(item);

            for (var layerIndex = 1; layerIndex < ForwardPropagators.Length; layerIndex++)
            {
                var fp = ForwardPropagators[layerIndex];
                fp.ComputeLayer();
            }

        }
    }
}
