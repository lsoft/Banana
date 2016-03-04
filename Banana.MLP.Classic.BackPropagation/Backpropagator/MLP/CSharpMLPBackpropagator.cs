using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banana.MLP.Backpropagator.Layer;
using Banana.MLP.Backpropagator.MLP;
using Banana.MLP.Classic.BackPropagation.Backpropagator.Layer;
using Banana.MLP.Classic.BackPropagation.DeDyAggregator;
using Banana.MLP.Classic.BackPropagation.DeDzCalculator;
using Banana.MLP.Classic.BackPropagation.DeDzCalculator.Hidden;
using Banana.MLP.Classic.BackPropagation.DeDzCalculator.Output;
using Banana.MLP.Classic.BackPropagation.UpdateNablaExecutor;
using Banana.MLP.Classic.BackPropagation.UpdateNeuronExecutor;
using Banana.MLP.Container.Layer.CSharp;
using Banana.MLP.Container.MLP;
using Banana.MLP.DesiredValues;
using Banana.MLP.LearningConfig;

namespace Banana.MLP.Classic.BackPropagation.Backpropagator.MLP
{
    public class CSharpMLPBackpropagator : IMLPBackpropagator
    {
        public ILayerBackpropagator[] BackPropagators
        {
            get;
            private set;
        }

        public CSharpMLPBackpropagator(
            IMLPContainer<CSharpLayerContainer> mlpContainer,
            ICSharpDesiredValuesContainer desiredValuesContainer,
            ILearningAlgorithmConfig learningAlgorithmConfig
            )
        {
            if (mlpContainer == null)
            {
                throw new ArgumentNullException("mlpContainer");
            }
            if (desiredValuesContainer == null)
            {
                throw new ArgumentNullException("desiredValuesContainer");
            }
            if (learningAlgorithmConfig == null)
            {
                throw new ArgumentNullException("learningAlgorithmConfig");
            }

            BackPropagators = new ILayerBackpropagator[mlpContainer.Layers.Length];

            //hidden layes
            for (var layerIndex = 1; layerIndex < mlpContainer.Layers.Length; layerIndex++)
            {
                var previousLayerContainer = mlpContainer.Layers[layerIndex - 1];
                var currentLayerContainer = mlpContainer.Layers[layerIndex];

                IDeDyAggregator deDyAggregator = new CSharpDeDyAggregator(
                    previousLayerContainer.Configuration,
                    currentLayerContainer
                    );

                IDeDzCalculator deDzCalculator;
                if (layerIndex == (mlpContainer.Layers.Length - 1))
                {
                    //last layer

                    deDzCalculator = new CSharpOutputLayerDeDzCalculator(
                        learningAlgorithmConfig,
                        currentLayerContainer,
                        desiredValuesContainer
                        );
                }
                else
                {
                    //hidden layer

                    var nextLayerContainer = mlpContainer.Layers[layerIndex + 1];

                    deDzCalculator = new CSharpHiddenLayerDeDzCalculator(
                        currentLayerContainer,
                        nextLayerContainer
                        );
                }

                IUpdateNablaExecutor updateNablaExecutor = new CSharpUpdateNablaExecutor(
                    previousLayerContainer,
                    currentLayerContainer,
                    learningAlgorithmConfig
                    );

                BackPropagators[layerIndex] = new CSharpLayerBackpropagator(
                    deDyAggregator,
                    deDzCalculator,
                    updateNablaExecutor
                    );
            }
        }

        public void Backpropagate(
            float learningRate,
            bool firstItemInBatch
            )
        {
            for (var layerIndex = BackPropagators.Length - 1; layerIndex > 0; layerIndex--)
            {
                var backpropagator = BackPropagators[layerIndex];

                backpropagator.Backpropagate(
                    learningRate,
                    firstItemInBatch
                    );
            }
        }

    }
}
