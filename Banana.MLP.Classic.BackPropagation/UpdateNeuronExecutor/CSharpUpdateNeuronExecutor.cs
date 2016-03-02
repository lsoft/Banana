using System;
using Banana.Common;
using Banana.MLP.Container.Layer.CSharp;
using Banana.MLP.LearningConfig;

namespace Banana.MLP.Classic.BackPropagation.UpdateNeuronExecutor
{
    public class CSharpUpdateNeuronExecutor : IUpdateNeuronExecutor
    {
        private readonly ICSharpLayerContainer _layerContainer;
        private readonly ILearningAlgorithmConfig _learningAlgorithmConfig;

        public CSharpUpdateNeuronExecutor(
            ICSharpLayerContainer layerContainer,
            ILearningAlgorithmConfig learningAlgorithmConfig
            )
        {
            if (layerContainer == null)
            {
                throw new ArgumentNullException("layerContainer");
            }
            if (learningAlgorithmConfig == null)
            {
                throw new ArgumentNullException("learningAlgorithmConfig");
            }
            _layerContainer = layerContainer;
            _learningAlgorithmConfig = learningAlgorithmConfig;
        }

        public void Execute(
            )
        {
            ForHelper.ForBetween(0, _layerContainer.WeightMem.Length, cc =>
            {
                _layerContainer.WeightMem[cc] += _layerContainer.NablaWeights[cc] / _learningAlgorithmConfig.BatchSize;
            }
            ); //ForHelper.ForBetween

            ForHelper.ForBetween(0, _layerContainer.BiasMem.Length, cc =>
            {
                _layerContainer.BiasMem[cc] += _layerContainer.NablaBiases[cc] / _learningAlgorithmConfig.BatchSize;
            }
            ); //ForHelper.ForBetween

        }

    }
}
