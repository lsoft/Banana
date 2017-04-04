using System;
using System.Runtime.CompilerServices;
using Banana.Common;
using Banana.Common.Others;
using Banana.MLP.Configuration.Layer;
using Banana.MLP.Container.Layer.CSharp;

namespace Banana.MLP.Classic.BackPropagation.DeDyAggregator
{
    public class CSharpDeDyAggregator2 : IDeDyAggregator
    {
        private readonly ILayerConfiguration _previousLayerConfiguration;
        private readonly ICSharpLayerContainer _aggregateLayerContainer;
        
        private readonly KahanAlgorithm.Accumulator[] _accumulators;

        public CSharpDeDyAggregator2(
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

            var accumulators = new KahanAlgorithm.Accumulator[_previousLayerConfiguration.TotalNeuronCount];
            accumulators.Fill(() => new KahanAlgorithm.Accumulator());

            _accumulators = accumulators;
        }

        public void Aggregate(
            )
        {
            var aggregateLayerTotalNeuronCount = _aggregateLayerContainer.Configuration.TotalNeuronCount;
            var previousLayerTotalNeuronCount = _previousLayerConfiguration.TotalNeuronCount;

            var aggregateLayerWeightMem = _aggregateLayerContainer.WeightMem;
            var aggregateLayerDeDzMem = _aggregateLayerContainer.DeDz;


            var cpuCount = Environment.ProcessorCount;
            var perThreadCount = previousLayerTotalNeuronCount / cpuCount;
            var ost = previousLayerTotalNeuronCount % cpuCount;
            if (ost > 0)
            {
                perThreadCount++;
            }

            ForHelper.ForBetween(0, cpuCount, cpuIndex =>
            {
                var startii = cpuIndex * perThreadCount;
                var endii = Math.Min(
                    (cpuIndex + 1) * perThreadCount,
                    previousLayerTotalNeuronCount
                    );

                #region first iteration doing Store instead of Add

                int nextWeightIndex_FirstLayer = ComputeWeightIndex(
                    previousLayerTotalNeuronCount,
                    0
                    ) + startii;

                float dedz0 = aggregateLayerDeDzMem[0];

                for (var nii = startii; nii < endii; nii++)
                {
                    var accDeDy = _accumulators[nii];

                    float w = aggregateLayerWeightMem[nextWeightIndex_FirstLayer]; //w is a dz/dy
                    float dedy = w * dedz0;

                    accDeDy.Store(dedy); // <-- Store instead of Add!

                    nextWeightIndex_FirstLayer++;
                }

                #endregion

                for (var aggregateNeuronIndex = 1; aggregateNeuronIndex < aggregateLayerTotalNeuronCount; ++aggregateNeuronIndex)
                {
                    float dedz = aggregateLayerDeDzMem[aggregateNeuronIndex];

                    int nextWeightIndex = ComputeWeightIndex(
                        previousLayerTotalNeuronCount,
                        aggregateNeuronIndex
                        ) + startii;

                    for (var nii = startii; nii < endii; nii++)
                    {
                        var accDeDy = _accumulators[nii];

                        float w = aggregateLayerWeightMem[nextWeightIndex]; //w is a dz/dy
                        float dedy = w*dedz;

                        accDeDy.Add(dedy);

                        nextWeightIndex++;
                    }
                }

                //сохраняем
                for (var nii = startii; nii < endii; nii++)
                {
                    var accDeDy = _accumulators[nii];

                    _aggregateLayerContainer.DeDy[nii] = accDeDy.Sum;
                }
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