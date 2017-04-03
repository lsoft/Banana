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
            var weightMem = _layerContainer.WeightMem;
            var nablaWeights = _layerContainer.NablaWeights;
            var batchSize = _learningAlgorithmConfig.BatchSize;

            ForHelper.ForBetween(0, _layerContainer.WeightMem.Length, cc =>
            {
                weightMem[cc] += nablaWeights[cc] / batchSize;
            }
            ); //ForHelper.ForBetween

            var biasMem = _layerContainer.BiasMem;
            var nablaBiases = _layerContainer.NablaBiases;

            ForHelper.ForBetween(0, _layerContainer.BiasMem.Length, cc =>
            {
                biasMem[cc] += nablaBiases[cc] / batchSize;
            }
            ); //ForHelper.ForBetween

        }

    }
}
