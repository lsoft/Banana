using System;
using System.Runtime.CompilerServices;
using Banana.Common;
using Banana.MLP.Container.Layer.CSharp;
using Banana.MLP.DesiredValues;
using Banana.MLP.LearningConfig;

namespace Banana.MLP.Classic.BackPropagation.UpdateNablaExecutor
{
    public class CSharpUpdateNablaExecutor : IUpdateNablaExecutor
    {
        private readonly ICSharpLayerContainer _previousLayerContainer;
        private readonly ICSharpLayerContainer _currentLayerContainer;
        private readonly ILearningAlgorithmConfig _learningAlgorithmConfig;

        public CSharpUpdateNablaExecutor(
            ICSharpLayerContainer previousLayerContainer,
            ICSharpLayerContainer currentLayerContainer,
            ILearningAlgorithmConfig learningAlgorithmConfig
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
            if (learningAlgorithmConfig == null)
            {
                throw new ArgumentNullException("learningAlgorithmConfig");
            }

            _previousLayerContainer = previousLayerContainer;
            _currentLayerContainer = currentLayerContainer;
            _learningAlgorithmConfig = learningAlgorithmConfig;
        }

        public void CalculateOverwrite(
            float learningRate
            )
        {
            Calculate(
                learningRate,
                AriphmeticHelper.Assign
                );
        }

        public void CalculateIncrement(
            float learningRate
            )
        {
            Calculate(
                learningRate,
                AriphmeticHelper.Add
                );
        }

        private void Calculate(
            float learningRate,
            AriphmeticHelper.FloatDelegate operation
            )
        {
            if (operation == null)
            {
                throw new ArgumentNullException("operation");
            }

            ForHelper.ForBetween(0, _currentLayerContainer.Configuration.TotalNeuronCount, neuronIndex =>
            {
                float dedz = _currentLayerContainer.DeDz[neuronIndex];

                int currentNablaIndex = ComputeWeightIndex(_previousLayerContainer.Configuration.TotalNeuronCount, neuronIndex);

                for (
                    int weightIndex = 0;
                    weightIndex < _previousLayerContainer.Configuration.TotalNeuronCount;
                    ++weightIndex)
                {
                    float prevOut = _previousLayerContainer.StateMem[weightIndex];

                    float weightRegularizationCoef = _learningAlgorithmConfig.RegularizationFactor * _currentLayerContainer.WeightMem[currentNablaIndex + weightIndex];
                    float weightCoef = prevOut + weightRegularizationCoef;
                    float deltaWeight = learningRate * dedz * weightCoef;

                    operation(ref _currentLayerContainer.NablaWeights[currentNablaIndex + weightIndex], deltaWeight);
                }

                float biasRegularizationCoef = _learningAlgorithmConfig.RegularizationFactor * _currentLayerContainer.BiasMem[neuronIndex];
                float biasCoef = 1 + biasRegularizationCoef;
                float deltaBias = learningRate * dedz * biasCoef;

                operation(ref _currentLayerContainer.NablaBiases[neuronIndex], deltaBias);
            }
            ); //ForHelper.ForBetween
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
