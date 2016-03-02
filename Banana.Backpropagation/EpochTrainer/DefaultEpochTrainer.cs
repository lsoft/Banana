﻿using System;
using Banana.Backpropagation.Propagators;
using Banana.Common;
using Banana.Common.Ambient;
using Banana.Data.Set;
using Banana.MLP.DesiredValues;
using Banana.MLP.LearningConfig;

namespace Banana.Backpropagation.EpochTrainer
{
    public class DefaultEpochTrainer : IEpochTrainer
    {
        private readonly ILearningAlgorithmConfig _learningAlgorithmConfig;
        private readonly IMLPPropagators _propagators;
        private readonly IDesiredValuesContainer _desiredValuesContainer;
        private readonly Action _batchAwaiterAction;

        public DefaultEpochTrainer(
            ILearningAlgorithmConfig learningAlgorithmConfig,
            IMLPPropagators propagators,
            IDesiredValuesContainer desiredValuesContainer,
            Action batchAwaiterAction
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
            if (batchAwaiterAction == null)
            {
                throw new ArgumentNullException("batchAwaiterAction");
            }

            _learningAlgorithmConfig = learningAlgorithmConfig;
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

                            #region forward pass

                            _propagators.ForwardPropagator.Propagate(trainDataItem);

                            #endregion

                            #region backward pass, error propagation

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

                    #region update weights and bias into opencl memory wrappers

                    if (batchProcessedItemCount > 0)
                    {
                        _propagators.UpdateNeuronExecutor.Execute();
                    }

                    #endregion

                    //Make sure we're done with everything that's been requested before
                    _batchAwaiterAction();

                }
            }

            ConsoleAmbientContext.Console.Write(new string(' ', 60));
            ConsoleAmbientContext.Console.ReturnCarriage();

            //конец эпохи обучения
        }
    }
}