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
                AriphmeticHelper.AssignConstant
                );
        }

        public void CalculateIncrement(
            float learningRate
            )
        {
            Calculate(
                learningRate,
                AriphmeticHelper.AddConstant
                );
        }

        private void Calculate(
            float learningRate,
            float oldValueScale
            )
        {
            ForHelper.ForBetween(0, _currentLayerContainer.Configuration.TotalNeuronCount, neuronIndex =>
            {
                float dedz = _currentLayerContainer.DeDz[neuronIndex];

                int currentNablaIndex = ComputeWeightIndex(_previousLayerContainer.Configuration.TotalNeuronCount, neuronIndex);

                var previousLayerTotalNeuronCount = _previousLayerContainer.Configuration.TotalNeuronCount;
                var regularizationFactor = _learningAlgorithmConfig.RegularizationFactor;

                var previousLayerStateMem = _previousLayerContainer.StateMem;
                var currentLayerWeightMem = _currentLayerContainer.WeightMem;
                var currentLayerNablaWeightsMem = _currentLayerContainer.NablaWeights;
                var currentLayerNablaBiasesMem = _currentLayerContainer.NablaBiases;

                var learningRateDedz = learningRate * dedz;

                for (
                    int weightIndex = 0;
                    weightIndex < previousLayerTotalNeuronCount;
                    ++weightIndex)
                {
                    float prevOut = previousLayerStateMem[weightIndex];

                    var nablaIndex = currentNablaIndex + weightIndex;

                    float weightRegularizationCoef = regularizationFactor * currentLayerWeightMem[nablaIndex];
                    float weightCoef = prevOut + weightRegularizationCoef;
                    float deltaWeight = learningRateDedz * weightCoef;

                    var oldWeight = currentLayerNablaWeightsMem[nablaIndex];
                    var newWeight = oldWeight*oldValueScale + deltaWeight;
                    currentLayerNablaWeightsMem[nablaIndex] = newWeight;
                }

                float biasRegularizationCoef = regularizationFactor * _currentLayerContainer.BiasMem[neuronIndex];
                float biasCoef = 1f + biasRegularizationCoef;
                float deltaBias = learningRateDedz * biasCoef;

                var oldBias = currentLayerNablaBiasesMem[neuronIndex];
                var newBias = oldBias * oldValueScale + deltaBias;
                currentLayerNablaBiasesMem[neuronIndex] = newBias;
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
