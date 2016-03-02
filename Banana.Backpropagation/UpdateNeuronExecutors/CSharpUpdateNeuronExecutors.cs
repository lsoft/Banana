using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banana.Common;
using Banana.MLP.Classic.BackPropagation.UpdateNeuronExecutor;
using Banana.MLP.Container.MLP;
using Banana.MLP.LearningConfig;

namespace Banana.Backpropagation.UpdateNeuronExecutors
{
    public class CSharpUpdateNeuronExecutors : IUpdateNeuronExecutors
    {
        public IUpdateNeuronExecutor[] UpdateNeuronExecutors
        {
            get;
            private set;
        }

        public CSharpUpdateNeuronExecutors(
            IMLPContainer mlpContainer,
            ILearningAlgorithmConfig learningAlgorithmConfig
            )
        {
            if (mlpContainer == null)
            {
                throw new ArgumentNullException("mlpContainer");
            }
            if (learningAlgorithmConfig == null)
            {
                throw new ArgumentNullException("learningAlgorithmConfig");
            }

            UpdateNeuronExecutors = new IUpdateNeuronExecutor[mlpContainer.Layers.Length];
            for (var layerIndex = 1; layerIndex < mlpContainer.Layers.Length; layerIndex++)
            {
                UpdateNeuronExecutors[layerIndex] = new CSharpUpdateNeuronExecutor(
                    mlpContainer.Layers[layerIndex],
                    learningAlgorithmConfig
                    );
            }
        }

        public void Execute(
            )
        {
            ForHelper.ForBetween(1, UpdateNeuronExecutors.Length, layerIndex =>
            {
                var une = UpdateNeuronExecutors[layerIndex];

                une.Execute();
            });// ForHelper.ForBetween
        }
    }
}
