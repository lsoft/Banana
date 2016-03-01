using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banana.Common;
using Banana.MLP.Container.CSharp;
using Banana.MLP.DesiredValues;
using Banana.MLP.LearningConfig;

namespace Banana.MLP.Classic.BackPropagation.CSharp.Kernel
{
    internal class HiddenLayerKernel
    {
        private readonly ILearningAlgorithmConfig _learningAlgorithmConfig;
        private readonly int _dataCount;

        public HiddenLayerKernel(
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

            ForHelper.ForBetween(0, currentLayerContainer.Configuration.TotalNeuronCount, neuronIndex =>
            {
                float dedy = currentLayerContainer.DeDy[neuronIndex];

                float z = currentLayerContainer.NetMem[neuronIndex];
                float dydz = currentLayerContainer.Configuration.LayerActivationFunction.ComputeFirstDerivative(z);
                var dedz = dedy * dydz;
                currentLayerContainer.DeDz[neuronIndex] = dedz;

                int currentNablaIndex = ComputeWeightIndex(previousLayerContainer.Configuration.TotalNeuronCount, neuronIndex);

                for (
                    int currentWeightIndex = 0;
                    currentWeightIndex < previousLayerContainer.Configuration.TotalNeuronCount; 
                    ++currentWeightIndex)
                {
                    float prevOut = previousLayerContainer.StateMem[currentWeightIndex];

                    float regularizationCoef = _learningAlgorithmConfig.RegularizationFactor * currentLayerContainer.WeightMem[currentWeightIndex] / _dataCount;
                    float coef = prevOut + regularizationCoef;
                    float deltaWeight = learningRate * dedz * coef;

                    currentLayerContainer.NablaWeights[currentNablaIndex + currentWeightIndex] = deltaWeight;
                }

                float deltaBias =
                    learningRate *
                    dedz *
                    (1 + _learningAlgorithmConfig.RegularizationFactor * currentLayerContainer.BiasMem[neuronIndex] / _dataCount);

                currentLayerContainer.NablaBiases[neuronIndex] = deltaBias;

            }
            ); // ForBetween
        }

        public void CalculateIncrement(
            ICSharpLayerContainer previousLayerContainer,
            ICSharpLayerContainer currentLayerContainer,
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

            ForHelper.ForBetween(0, currentLayerContainer.Configuration.TotalNeuronCount, neuronIndex =>
            {
                float dedy = currentLayerContainer.DeDy[neuronIndex];

                float z = currentLayerContainer.NetMem[neuronIndex];
                float dydz = currentLayerContainer.Configuration.LayerActivationFunction.ComputeFirstDerivative(z);
                var dedz = dedy * dydz;
                currentLayerContainer.DeDz[neuronIndex] = dedz;

                int currentNablaIndex = ComputeWeightIndex(previousLayerContainer.Configuration.TotalNeuronCount, neuronIndex);

                for (
                    int currentWeightIndex = 0;
                    currentWeightIndex < previousLayerContainer.Configuration.TotalNeuronCount;
                    ++currentWeightIndex)
                {
                    float prevOut = previousLayerContainer.StateMem[currentWeightIndex];

                    float regularizationCoef = _learningAlgorithmConfig.RegularizationFactor * currentLayerContainer.WeightMem[currentWeightIndex] / _dataCount;
                    float coef = prevOut + regularizationCoef;
                    float deltaWeight = learningRate * dedz * coef;

                    currentLayerContainer.NablaWeights[currentNablaIndex + currentWeightIndex] += deltaWeight;
                }

                float deltaBias =
                    learningRate *
                    dedz *
                    (1 + _learningAlgorithmConfig.RegularizationFactor * currentLayerContainer.BiasMem[neuronIndex] / _dataCount);

                currentLayerContainer.NablaBiases[neuronIndex] += deltaBias;

            }
            ); // ForBetween
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
