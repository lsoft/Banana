using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
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
                var aggregateLayerTotalNeuronCount = _aggregateLayerContainer.Configuration.TotalNeuronCount;
                var previousLayerTotalNeuronCount = _previousLayerConfiguration.TotalNeuronCount;

                var aggregateLayerWeightMem = _aggregateLayerContainer.WeightMem;
                var aggregateLayerDeDzMem = _aggregateLayerContainer.DeDz;

                //просчет состо€ни€ нейронов текущего сло€, по состо€нию нейронов последующего (with Kahan Algorithm)
                var accDeDy = new KahanAlgorithm.Accumulator();
                for (var aggregateNeuronIndex = 0; aggregateNeuronIndex < aggregateLayerTotalNeuronCount; ++aggregateNeuronIndex)
                {
                    int nextWeightIndex = ComputeWeightIndex(
                        previousLayerTotalNeuronCount,
                        aggregateNeuronIndex
                        ) + previousLayerNeuronIndex; //не векторизуетс€:(

                    float w = aggregateLayerWeightMem[nextWeightIndex]; //w is a dz/dy
                    float dedz = aggregateLayerDeDzMem[aggregateNeuronIndex];
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