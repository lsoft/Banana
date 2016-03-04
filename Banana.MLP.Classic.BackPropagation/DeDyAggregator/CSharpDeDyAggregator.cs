using System;
using System.Runtime.CompilerServices;
using Banana.Common;
using Banana.Common.Others;
using Banana.MLP.Configuration.Layer;
using Banana.MLP.Container.Layer.CSharp;

namespace Banana.MLP.Classic.BackPropagation.DeDyAggregator
{
    public class CSharpDeDyAggregator : IDeDyAggregator
    {
        private readonly ILayerConfiguration _previousLayerConfiguration;
        private readonly ICSharpLayerContainer _aggregateLayerContainer;

        public CSharpDeDyAggregator(
            ILayerConfiguration previousLayerConfiguration,
            ICSharpLayerContainer aggregateLayerContainer
            )
        {
            if (previousLayerConfiguration == null)
            {
                throw new ArgumentNullException("previousLayerConfiguration");
            }
            if (aggregateLayerContainer == null)
            {
                throw new ArgumentNullException("aggregateLayerContainer");
            }
            if ((previousLayerConfiguration.TotalNeuronCount * aggregateLayerContainer.Configuration.TotalNeuronCount) != aggregateLayerContainer.WeightMem.Length)
            {
                throw new ArgumentException("(previousLayerConfiguration.TotalNeuronCount * aggregateLayerContainer.Configuration.TotalNeuronCount) != aggregateLayerContainer.WeightMem.Length");
            }

            _previousLayerConfiguration = previousLayerConfiguration;
            _aggregateLayerContainer = aggregateLayerContainer;
        }

        public void Aggregate(
            )
        {
            ForHelper.ForBetween(0, _previousLayerConfiguration.TotalNeuronCount, previousLayerNeuronIndex =>
            {
                //просчет состо€ни€ нейронов текущего сло€, по состо€нию нейронов последующего (with Kahan Algorithm)
                var accDeDy = new KahanAlgorithm.Accumulator();
                for (var aggregateNeuronIndex = 0; aggregateNeuronIndex < _aggregateLayerContainer.Configuration.TotalNeuronCount; ++aggregateNeuronIndex)
                {
                    int nextWeightIndex = ComputeWeightIndex(
                        _previousLayerConfiguration.TotalNeuronCount,
                        aggregateNeuronIndex
                        ) + previousLayerNeuronIndex; //не векторизуетс€:(

                    float w = _aggregateLayerContainer.WeightMem[nextWeightIndex]; //w is a dz/dy
                    float dedz = _aggregateLayerContainer.DeDz[aggregateNeuronIndex];
                    float dedy = w * dedz;

                    accDeDy.Add(dedy);
                }

                _aggregateLayerContainer.DeDy[previousLayerNeuronIndex] = accDeDy.Sum;
            }); //ForHelper.ForBetween
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int ComputeWeightIndex(
            int previousLayerNeuronCount,
            int neuronIndex)
        {
            return
                previousLayerNeuronCount * neuronIndex;
        }

    }
}