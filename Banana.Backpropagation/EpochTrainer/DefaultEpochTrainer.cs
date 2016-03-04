using System;
using Banana.Backpropagation.Config;
using Banana.Backpropagation.Propagators;
using Banana.Common;
using Banana.Common.Ambient;
using Banana.Common.Others;
using Banana.Data.Set;
using Banana.MLP.Container.Layer.CSharp;
using Banana.MLP.DesiredValues;
using Banana.MLP.DesiredValues.DataSetIterator;
using Banana.MLP.LearningConfig;

namespace Banana.Backpropagation.EpochTrainer
{
    public class DefaultEpochTrainer<T> : IEpochTrainer
        where T : ILayerContainer
    {
        private readonly ILearningAlgorithmConfig _learningAlgorithmConfig;
        private readonly IBackpropagationConfig _backpropagationConfig;
        private readonly IMLPPropagators<T> _propagators;
        private readonly IDesiredValuesContainer _desiredValuesContainer;
        private readonly Action _batchAwaiterAction;

        public DefaultEpochTrainer(
            ILearningAlgorithmConfig learningAlgorithmConfig,
            IBackpropagationConfig backpropagationConfig,
            IMLPPropagators<T> propagators,
            IDesiredValuesContainer desiredValuesContainer,
            Action batchAwaiterAction
            )
        {
            if (learningAlgorithmConfig == null)
            {
                throw new ArgumentNullException("learningAlgorithmConfig");
            }
            if (backpropagationConfig == null)
            {
                throw new ArgumentNullException("backpropagationConfig");
            }
            if (propagators == null)
            {
                throw new ArgumentNullException("propagators");
            }
            if (desiredValuesContainer == null)
            {
                throw new ArgumentNullException("desiredValuesContainer");
            }
            if (batchAwaiterAction == null)
            {
                throw new ArgumentNullException("batchAwaiterAction");
            }

            _learningAlgorithmConfig = learningAlgorithmConfig;
            _backpropagationConfig = backpropagationConfig;
            _propagators = propagators;
            _desiredValuesContainer = desiredValuesContainer;
            _batchAwaiterAction = batchAwaiterAction;
        }

        public void Train(
            IDataSet data,
            float learningRate
            )
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            var totalProcessedItemCount = 0;
            using (var iterator = data.StartIterate())
            using (var qiterator = new DesiredValuesContainerIterator(_backpropagationConfig.ItemCountCacheSize, _desiredValuesContainer, iterator))
            {
                foreach (var batch in qiterator.LazySplit(_learningAlgorithmConfig.BatchSize))
                {
                    var batchProcessedItemCount = 0;
                    foreach (var trainDataItem in batch)
                    {
                        var firstItemInBatch = batchProcessedItemCount == 0;

                        #region -> and <- propagate

                        _propagators.ForwardPropagator.Propagate(trainDataItem);

                        _desiredValuesContainer.SetValues(trainDataItem.Output);

                        _propagators.Backpropagator.Backpropagate(
                            learningRate,
                            firstItemInBatch
                            );

                        #endregion

                        #region logging

                        const int ItemCountBetweenLogs = 100;

                        var logStep = data.Count/ItemCountBetweenLogs;
                        if (logStep > 0 && totalProcessedItemCount%logStep == 0)
                        {
                            ConsoleAmbientContext.Console.Write(
                                "Epoche progress: {0}%, {1}      ",
                                ((long) totalProcessedItemCount*100/data.Count),
                                DateTime.Now.ToString()
                                );

                            ConsoleAmbientContext.Console.ReturnCarriage();
                        }

                        #endregion

                        batchProcessedItemCount++;
                        totalProcessedItemCount++;
                    }

                    #region update weights and bias

                    if (batchProcessedItemCount > 0)
                    {
                        _propagators.UpdateNeuronExecutor.Execute();
                    }

                    #endregion

                    //Make sure we're done with everything that's been requested before
                    _batchAwaiterAction();
                }
            }

            /*
            //process data set
            using (var enumerator = data.StartIterate())
            {
                var allowedToContinue = true;
                for (
                    var currentIndex = 0;
                    allowedToContinue;
                    currentIndex += _learningAlgorithmConfig.BatchSize
                    )
                {
                    var batchProcessedItemCount = 0;

                    //process one batch
                    for (
                        var inBatchIndex = 0;
                        inBatchIndex < _learningAlgorithmConfig.BatchSize && allowedToContinue;
                        ++inBatchIndex
                        )
                    {
                        allowedToContinue = enumerator.MoveNext();
                        if (allowedToContinue)
                        {
                            var firstItemInBatch = inBatchIndex == 0;

                            var trainDataItem = enumerator.Current;

                            #region -> and <- propagate

                            _propagators.ForwardPropagator.Propagate(trainDataItem);

                            _desiredValuesContainer.SetValues(trainDataItem.Output);

                            _propagators.Backpropagator.Backpropagate(
                                learningRate,
                                firstItemInBatch
                                );

                            #endregion

                            #region logging

                            const int ItemCountBetweenLogs = 100;

                            var logStep = data.Count / ItemCountBetweenLogs;
                            if (logStep > 0 && currentIndex % logStep == 0)
                            {
                                ConsoleAmbientContext.Console.Write(
                                    "Epoche progress: {0}%, {1}      ",
                                    ((long)currentIndex * 100 / data.Count),
                                    DateTime.Now.ToString()
                                    );

                                ConsoleAmbientContext.Console.ReturnCarriage();
                            }

                            #endregion

                            batchProcessedItemCount++;
                        }
                    }

                    #region update weights and bias

                    if (batchProcessedItemCount > 0)
                    {
                        _propagators.UpdateNeuronExecutor.Execute();
                    }

                    #endregion

                    //Make sure we're done with everything that's been requested before
                    _batchAwaiterAction();
                }
            }
            //*/

            ConsoleAmbientContext.Console.Write(new string(' ', 60));
            ConsoleAmbientContext.Console.ReturnCarriage();

            //конец эпохи обучения
        }
    }
}
