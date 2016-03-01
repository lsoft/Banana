using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banana.Common;
using Banana.MLP.Container.CSharp;
using Banana.MLP.DesiredValues;
using Banana.MLP.Layer;
using Banana.MLP.LearningConfig;

namespace Banana.MLP.Classic.BackPropagation.CSharp.Kernel
{
    internal class OutputLayerKernel
    {
        private readonly ILearningAlgorithmConfig _learningAlgorithmConfig;
        private readonly int _dataCount;

        public OutputLayerKernel(
            ILearningAlgorithmConfig learningAlgorithmConfig,
            int dataCount
            )
        {
            if (learningAlgorithmConfig == null)
            {
                throw new ArgumentNullException("learningAlgorithmConfig");
            }
            _learningAlgorithmConfig = learningAlgorithmConfig;
            _dataCount = dataCount;
        }

        public void CalculateOverwrite(
            ICSharpLayerContainer previousLayerContainer,
            ICSharpLayerContainer currentLayerContainer,
            ICSharpDesiredValuesContainer desiredValuesContainer,
            float learningRate
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
            if (desiredValuesContainer == null)
            {
                throw new ArgumentNullException("desiredValuesContainer");
            }

            ForHelper.ForBetween(0, currentLayerContainer.Configuration.TotalNeuronCount, neuronIndex =>
            {
                float z = currentLayerContainer.NetMem[neuronIndex];
                float dydz = currentLayerContainer.Configuration.LayerActivationFunction.ComputeFirstDerivative(z);

                float dedy = _learningAlgorithmConfig.TargetMetrics.CalculatePartialDerivativeByV2Index(
                    currentLayerContainer.StateMem,
                    desiredValuesContainer.DesiredOutput,
                    neuronIndex
                    );

                float dedz = dedy * dydz;

                currentLayerContainer.DeDz[neuronIndex] = dedz;

                int nablaNeuronShift = ComputeWeightIndex(previousLayerContainer.Configuration.TotalNeuronCount, neuronIndex);

                for (
                    int weightIndex = 0;
                    weightIndex < previousLayerContainer.Configuration.TotalNeuronCount;
                    ++weightIndex)
                {
                    float deltaWeight =
                        learningRate *
                        dedz *
                        (previousLayerContainer.StateMem[weightIndex] + _learningAlgorithmConfig.RegularizationFactor * currentLayerContainer.WeightMem[nablaNeuronShift + weightIndex] / _dataCount);

                    currentLayerContainer.NablaWeights[nablaNeuronShift + weightIndex] = deltaWeight;
                }

                float deltaBias =
                    learningRate *
                    dedz *
                    (1 + _learningAlgorithmConfig.RegularizationFactor * currentLayerContainer.BiasMem[neuronIndex] / _dataCount);

                currentLayerContainer.NablaBiases[neuronIndex] = deltaBias;
            }
            ); //ForHelper.ForBetween
        }

        public void CalculateIncrement(
            ICSharpLayerContainer previousLayerContainer,
            ICSharpLayerContainer currentLayerContainer,
            ICSharpDesiredValuesContainer desiredValuesContainer,
            float learningRate
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
            if (desiredValuesContainer == null)
            {
                throw new ArgumentNullException("desiredValuesContainer");
            }

            ForHelper.ForBetween(0, currentLayerContainer.Configuration.TotalNeuronCount, neuronIndex =>
            {
                float z = currentLayerContainer.NetMem[neuronIndex];
                float dydz = currentLayerContainer.Configuration.LayerActivationFunction.ComputeFirstDerivative(z);

                float dedy = _learningAlgorithmConfig.TargetMetrics.CalculatePartialDerivativeByV2Index(
                    currentLayerContainer.StateMem,
                    desiredValuesContainer.DesiredOutput,
                    neuronIndex
                    );

                float dedz = dedy * dydz;

                currentLayerContainer.DeDz[neuronIndex] = dedz;

                int nablaNeuronShift = ComputeWeightIndex(previousLayerContainer.Configuration.TotalNeuronCount, neuronIndex);

                for (
                    int weightIndex = 0;
                    weightIndex < previousLayerContainer.Configuration.TotalNeuronCount;
                    ++weightIndex)
                {
                    float deltaWeight =
                        learningRate *
                        dedz *
                        (previousLayerContainer.StateMem[weightIndex] + _learningAlgorithmConfig.RegularizationFactor * currentLayerContainer.WeightMem[nablaNeuronShift + weightIndex] / _dataCount);

                    currentLayerContainer.NablaWeights[nablaNeuronShift + weightIndex] += deltaWeight;
                }

                float deltaBias =
                    learningRate *
                    dedz *
                    (1 + _learningAlgorithmConfig.RegularizationFactor * currentLayerContainer.BiasMem[neuronIndex] / _dataCount);

                currentLayerContainer.NablaBiases[neuronIndex] += deltaBias;
            }
            ); //ForHelper.ForBetween
        }

        private static int ComputeWeightIndex(
            int previousLayerNeuronCount,
            int neuronIndex)
        {
            return
                previousLayerNeuronCount * neuronIndex;
        }

    }
}
