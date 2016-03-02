using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banana.Backpropagation.Propagators;
using Banana.Common.Ambient;
using Banana.Data.SetProvider;
using Banana.MLP.AccuracyRecord;
using Banana.MLP.DesiredValues;
using Banana.MLP.LearningConfig;

namespace Banana.Backpropagation.Algorithm
{
    public class BackpropagationAlgorithm
    {
        private readonly ILearningAlgorithmConfig _learningAlgorithmConfig;
        private readonly IMLPPropagators _propagators;
        private readonly IDesiredValuesContainer _desiredValuesContainer;

        public BackpropagationAlgorithm(
            ILearningAlgorithmConfig learningAlgorithmConfig,
            IMLPPropagators propagators,
            IDesiredValuesContainer desiredValuesContainer
            )
        {
            if (learningAlgorithmConfig == null)
            {
                throw new ArgumentNullException("learningAlgorithmConfig");
            }
            if (propagators == null)
            {
                throw new ArgumentNullException("propagators");
            }
            if (desiredValuesContainer == null)
            {
                throw new ArgumentNullException("desiredValuesContainer");
            }
            _learningAlgorithmConfig = learningAlgorithmConfig;
            _propagators = propagators;
            _desiredValuesContainer = desiredValuesContainer;
        }

        public IAccuracyRecord Train(
            IDataSetProvider dataSetProvider
            )
        {
            if (dataSetProvider == null)
            {
                throw new ArgumentNullException("dataSetProvider");
            }

            ConsoleAmbientContext.Console.WriteLine(
                "BACKPROPAGATION STARTED WITH {0}",
                _propagators.MLPContainer.Configuration.GetLayerInformation()
                );

            #region валидируем дефолтовую сеть

            var beforeDefaultValidation = DateTime.Now;

            //ConsoleAmbientContext.Console.WriteLine("Default net validation results:");

            //var preTrainContainer = _artifactContainer.GetChildContainer("_pretrain");

            //_validation.Validate(
            //    _inferenceForwardPropagation,
            //    null,
            //    preTrainContainer
            //    );

            var afterDefaultValidation = DateTime.Now;

            ConsoleAmbientContext.Console.WriteLine("Default net validation takes {0}", (afterDefaultValidation - beforeDefaultValidation));

            #endregion



        }
    }
}
