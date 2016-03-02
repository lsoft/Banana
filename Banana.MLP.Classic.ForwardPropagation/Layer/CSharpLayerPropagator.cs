using System;
using Banana.Common;
using Banana.Common.Others;
using Banana.MLP.Container.Layer.CSharp;
using Banana.MLP.Propagator.Layer;

namespace Banana.MLP.Classic.ForwardPropagation.Layer
{
    public class CSharpLayerPropagator : ILayerPropagator
    {
        private readonly ICSharpLayerContainer _previousLayerContainer;
        private readonly ICSharpLayerContainer _currentLayerContainer;

        public ILayerContainer LayerContainer
        {
            get
            {
                return
                    _currentLayerContainer;
            }
        }

        public CSharpLayerPropagator(
            ICSharpLayerContainer previousLayerContainer,
            ICSharpLayerContainer currentLayerContainer
            )
        {
            if (previousLayerContainer == null)
            {
                throw new ArgumentNullException("previousLayerContainer");
            }
            if (currentLayerContainer == null)
            {
                throw new ArgumentNullException("currentLayerContainer");
            }

            //_currentLayer = currentLayer as IFullConnectedLayer;
            _previousLayerContainer = previousLayerContainer;
            _currentLayerContainer = currentLayerContainer;
        }


        public void ComputeLayer()
        {
            ForHelper.ForBetween(0, _currentLayerContainer.Configuration.TotalNeuronCount, neuronIndex =>
            {
                var previousLayerNeuronCountTotal = _previousLayerContainer.Configuration.TotalNeuronCount;

                var weightIndex = ComputeWeightIndex(previousLayerNeuronCountTotal, neuronIndex);

                //compute LastNET
                //instead of plain summation it's better to use Kahan algorithm due to better precision in floating point ariphmetics

                var acc = new KahanAlgorithm.Accumulator();

                for (var plnIndex = 0; plnIndex < previousLayerNeuronCountTotal; ++plnIndex)
                {
                    var increment =
                        _currentLayerContainer.WeightMem[weightIndex++]
                        * _previousLayerContainer.StateMem[plnIndex];

                    acc.Add(increment);
                }

                var lastNET = acc.Sum + _currentLayerContainer.BiasMem[neuronIndex];

                _currentLayerContainer.NetMem[neuronIndex] = lastNET;

                //compute last state
                var lastState = _currentLayerContainer.Configuration.LayerActivationFunction.Compute(lastNET);
                _currentLayerContainer.StateMem[neuronIndex] = lastState;
            }
            ); //ForHelper.ForBetween
        }

        public void WaitForCalculationFinished()
        {
            //nothing to do
        }

        private int ComputeWeightIndex(
           int previousLayerNeuronCount,
           int neuronIndex
            )
        {
            return
                previousLayerNeuronCount * neuronIndex;
        }
    }
}
