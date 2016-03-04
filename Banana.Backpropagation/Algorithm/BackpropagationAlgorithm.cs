using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banana.Backpropagation.EpochTrainer;
using Banana.Backpropagation.Propagators;
using Banana.Common.Ambient;
using Banana.Common.LearningRate;
using Banana.Common.Others;
using Banana.Data.SetProvider;
using Banana.MLP.AccuracyRecord;
using Banana.MLP.ArtifactContainer;
using Banana.MLP.Container.Layer.CSharp;
using Banana.MLP.DesiredValues;
using Banana.MLP.LearningConfig;
using Banana.MLP.MLPContainer;
using Banana.MLP.Validation;

namespace Banana.Backpropagation.Algorithm
{
    public class BackpropagationAlgorithm<T>
        where T : ILayerContainer
    {
        private readonly ILearningAlgorithmConfig _learningAlgorithmConfig;
        private readonly ILearningRate _learningRate;
        private readonly IMLPPropagators<T> _propagators;
        private readonly IMLPContainerHelper _mlpContainerHelper;
        private readonly IArtifactContainer _artifactContainer;
        private readonly IValidation _validation;
        private readonly IEpochTrainer _epochTrainer;

        public BackpropagationAlgorithm(
            ILearningAlgorithmConfig learningAlgorithmConfig,
            ILearningRate learningRate,
            IMLPPropagators<T> propagators,
            IMLPContainerHelper mlpContainerHelper,
            IArtifactContainer artifactContainer,
            IValidation validation,
            IEpochTrainer epochTrainer
            )
        {
            if (learningAlgorithmConfig == null)
            {
                throw new ArgumentNullException("learningAlgorithmConfig");
            }
            if (learningRate == null)
            {
                throw new ArgumentNullException("learningRate");
            }
            if (propagators == null)
            {
                throw new ArgumentNullException("propagators");
            }
            if (mlpContainerHelper == null)
            {
                throw new ArgumentNullException("mlpContainerHelper");
            }
            if (artifactContainer == null)
            {
                throw new ArgumentNullException("artifactContainer");
            }
            if (validation == null)
            {
                throw new ArgumentNullException("validation");
            }
            if (epochTrainer == null)
            {
                throw new ArgumentNullException("epochTrainer");
            }
            _learningAlgorithmConfig = learningAlgorithmConfig;
            _learningRate = learningRate;
            _propagators = propagators;
            _mlpContainerHelper = mlpContainerHelper;
            _artifactContainer = artifactContainer;
            _validation = validation;
            _epochTrainer = epochTrainer;
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

            ConsoleAmbientContext.Console.WriteLine("Default net validation results:");

            var preTrainContainer = _artifactContainer.GetChildContainer("_pretrain");

            _validation.Validate(
                _propagators.ForwardPropagator,
                null,
                preTrainContainer
                );

            var afterDefaultValidation = DateTime.Now;

            ConsoleAmbientContext.Console.WriteLine("Default net validation takes {0}", (afterDefaultValidation - beforeDefaultValidation));

            #endregion

            IAccuracyRecord result = null;
            var epochNumber = 0;

            //цикл по эпохам
            do
            {
                ConsoleAmbientContext.Console.WriteLine(
                "---------------------------------------- Epoch #{0} --------------------------------------",
                    epochNumber.ToString("D7"));

                ConsoleAmbientContext.Console.WriteLine("Current time: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                //скорость обучения на эту эпоху
                var learningRate = _learningRate.GetLearningRate(epochNumber);
                ConsoleAmbientContext.Console.WriteLine("Epoch learning rate: " + learningRate);

                #region data set preparation

                var preparationStart = DateTime.Now;

                ConsoleAmbientContext.Console.WriteLine("Epoch train data set preparation...");

                //запрашиваем данные (уже перемешанные)
                var trainData = dataSetProvider.GetDataSet(epochNumber);

                var lastLayerTotalNeuronCount = _propagators.MLPContainer.Layers.Last().Configuration.TotalNeuronCount;
                if (trainData.OutputLength != lastLayerTotalNeuronCount)
                {
                    ConsoleAmbientContext.Console.WriteWarning(
                        string.Format(
                            "На последнем слое сети {1} нейронов, а у датасета OutputLength = {0}",
                            trainData.OutputLength,
                            lastLayerTotalNeuronCount
                            )
                        );
                }

                var preparationEnd = DateTime.Now;

                #endregion

                #region train epoche

                //создаем папку эпохи
                var epocheContainer = _artifactContainer.GetChildContainer(
                    string.Format("epoche {0}", epochNumber)
                    );

                var trainTimeStart = DateTime.Now;

                //обучаем эпоху
                _epochTrainer.Train(
                    trainData,
                    learningRate
                    );

                //сколько времени заняла эпоха обучения
                var trainTimeEnd = DateTime.Now;

                #endregion

                #region validation

                var validationStart = DateTime.Now;

                //внешняя функция для обсчета на тестовом множестве
                var epocheAccuracyRecord = _validation.Validate(
                    _propagators.ForwardPropagator,
                    epochNumber,
                    epocheContainer
                    );
                var currentError = epocheAccuracyRecord.PerItemError;

                var needToSaveMLP = (result == null || (epocheAccuracyRecord.IsBetterThan(result)));

                var validationEnd = DateTime.Now;

                #endregion

                #region report epoche results

                ConsoleAmbientContext.Console.WriteLine(
                    "                  =========== Per-item error = {0} ===========",
                    (currentError >= float.MaxValue ? "не вычислено" : DoubleConverter.ToExactString(currentError))
                    );

                ConsoleAmbientContext.Console.WriteLine("Current time: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                ConsoleAmbientContext.Console.WriteLine(
                    string.Format(
                        "   Total: {0}  Preparation: {1}  Train: {2}  Validation: {3}",
                        (int)((validationEnd - preparationStart).TotalMilliseconds),
                        (int)((preparationEnd - preparationStart).TotalMilliseconds),
                        (int)((trainTimeEnd - trainTimeStart).TotalMilliseconds),
                        (int)((validationEnd - validationStart).TotalMilliseconds)
                        )
                    );

                if (needToSaveMLP)
                {
                    ConsoleAmbientContext.Console.WriteWarning("Saved!");
                }

                ConsoleAmbientContext.Console.WriteLine(
                    "----------------------------------------------------------------------------------------------");
                ConsoleAmbientContext.Console.WriteLine(string.Empty);

                #endregion

                #region save mlp if better

                if (needToSaveMLP)
                {
                    result = epocheAccuracyRecord;

                    _mlpContainerHelper.Save(
                        epocheContainer,
                        _propagators.MLPContainer,
                        epocheAccuracyRecord
                        );
                }

                #endregion

                epochNumber++;

                GC.Collect(0);
                GC.WaitForPendingFinalizers();
                GC.Collect(0);
            } while (epochNumber < _learningAlgorithmConfig.MaxEpoches);

            return
                result;
        }
    }
}
