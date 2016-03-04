using System;
using Banana.Common;
using Banana.MLP.Container.Layer.CSharp;

namespace Banana.MLP.Classic.BackPropagation.DeDzCalculator.Hidden
{
    public class CSharpHiddenLayerDeDzCalculator : IDeDzCalculator
    {
        private readonly ICSharpLayerContainer _currentLayerContainer;
        private readonly ICSharpLayerContainer _nextLayerContainer;

        public CSharpHiddenLayerDeDzCalculator(
            ICSharpLayerContainer currentLayerContainer,
            ICSharpLayerContainer nextLayerContainer
            )
        {
            if (currentLayerContainer == null)
            {
                throw new ArgumentNullException("currentLayerContainer");
            }
            if (nextLayerContainer == null)
            {
                throw new ArgumentNullException("nextLayerContainer");
            }

            _currentLayerContainer = currentLayerContainer;
            _nextLayerContainer = nextLayerContainer;
        }

        public void Calculate(
            )
        {
            ForHelper.ForBetween(0, _currentLayerContainer.Configuration.TotalNeuronCount, neuronIndex =>
            {
                float dedy = _nextLayerContainer.DeDy[neuronIndex];

                float z = _currentLayerContainer.NetMem[neuronIndex];
                float dydz = _currentLayerContainer.Configuration.LayerActivationFunction.ComputeFirstDerivative(z);
                
                var dedz = dedy * dydz;

                _currentLayerContainer.DeDz[neuronIndex] = dedz;
            }
            ); //ForHelper.ForBetween
        }
    }
}
