using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banana.Backpropagation.UpdateNeuronExecutors;
using Banana.MLP.Backpropagator.MLP;
using Banana.MLP.Classic.BackPropagation.Backpropagator;
using Banana.MLP.Classic.BackPropagation.Backpropagator.MLP;
using Banana.MLP.Classic.BackPropagation.DeDyAggregator;
using Banana.MLP.Classic.BackPropagation.DeDzCalculator;
using Banana.MLP.Classic.BackPropagation.DeDzCalculator.Hidden;
using Banana.MLP.Classic.BackPropagation.UpdateNablaExecutor;
using Banana.MLP.Classic.BackPropagation.UpdateNeuronExecutor;
using Banana.MLP.Classic.ForwardPropagation;
using Banana.MLP.Classic.ForwardPropagation.MLP;
using Banana.MLP.Container.MLP;
using Banana.MLP.DesiredValues;
using Banana.MLP.LearningConfig;
using Banana.MLP.Propagator.MLP;

namespace Banana.Backpropagation.Propagators
{
    public class CSharpMLPPropagators : IMLPPropagators
    {
        public IMLPContainer MLPContainer
        {
            get;
            private set;
        }

        public IMLPPropagator ForwardPropagator
        {
            get;
            private set;
        }

        public IMLPBackpropagator Backpropagator
        {
            get;
            private set;
        }

        public IUpdateNeuronExecutors UpdateNeuronExecutor
        {
            get;
            private set;
        }

        public CSharpMLPPropagators(
            IMLPContainer mlpContainer,
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

            MLPContainer = mlpContainer;

            ForwardPropagator = new CSharpMLPPropagator(
                mlpContainer
                );

            Backpropagator = new CSharpMLPBackpropagator(
                mlpContainer,
                desiredValuesContainer,
                learningAlgorithmConfig
                );

            UpdateNeuronExecutor = new CSharpUpdateNeuronExecutors(
                mlpContainer,
                learningAlgorithmConfig
                );
        }


    }
}
