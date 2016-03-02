using System;
using Banana.Common;
using Banana.MLP.Container.Layer.CSharp;
using Banana.MLP.DesiredValues;
using Banana.MLP.LearningConfig;

namespace Banana.MLP.Classic.BackPropagation.DeDzCalculator.Output
{
    public class CSharpOutputLayerDeDzCalculator : IDeDzCalculator
    {
        private readonly ILearningAlgorithmConfig _learningAlgorithmConfig;
        private readonly ICSharpLayerContainer _currentLayerContainer;
        private readonly ICSharpDesiredValuesContainer _desiredValuesContainer;

        public CSharpOutputLayerDeDzCalculator(
            ILearningAlgorithmConfig learningAlgorithmConfig,
            ICSharpLayerContainer currentLayerContainer,
            ICSharpDesiredValuesContainer desiredValuesContainer
            )
        {
            if (learningAlgorithmConfig == null)
            {
                throw new ArgumentNullException("learningAlgorithmConfig");
            }
            if (currentLayerContainer == null)
            {
                throw new ArgumentNullException("currentLayerContainer");
            }
            if (desiredValuesContainer == null)
            {
                throw new ArgumentNullException("desiredValuesContainer");
            }

            _learningAlgorithmConfig = learningAlgorithmConfig;
            _currentLayerContainer = currentLayerContainer;
            _desiredValuesContainer = desiredValuesContainer;
        }

        public void Calculate(
            )
        {
            ForHelper.ForBetween(0, _currentLayerContainer.Configuration.TotalNeuronCount, neuronIndex =>
            {
                float z = _currentLayerContainer.NetMem[neuronIndex];
                float dydz = _currentLayerContainer.Configuration.LayerActivationFunction.ComputeFirstDerivative(z);

                float dedy = _learningAlgorithmConfig.TargetMetrics.CalculatePartialDerivativeByV2Index(
                    _currentLayerContainer.StateMem,
                    _desiredValuesContainer.DesiredOutput,
                    neuronIndex
                    );

                float dedz = dedy * dydz;

                _currentLayerContainer.DeDz[neuronIndex] = dedz;
            }
            ); //ForHelper.ForBetween
        }
    }
}