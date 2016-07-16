using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Banana.Backpropagation.Config;
using Banana.Backpropagation.Propagators;
using Banana.Common;
using Banana.Common.Ambient;
using Banana.Common.Others;
using Banana.Data.Item;
using Banana.Data.Set;
using Banana.MLP.Container.Layer.CSharp;
using Banana.MLP.DesiredValues;
using Banana.MLP.DesiredValues.DataSetIterator;
using Banana.MLP.LearningConfig;

namespace Banana.Backpropagation.EpochTrainer
{
    public class DefaultEpochTrainer : IEpochTrainer
    {
        private readonly ILearningAlgorithmConfig _learningAlgorithmConfig;
        private readonly IBackpropagationConfig _backpropagationConfig;
        private readonly IMLPPropagators _propagators;
        private readonly IDesiredValuesContainer _desiredValuesContainer;
        private readonly Action _batchAwaiterAction;

        public DefaultEpochTrainer(
            ILearningAlgorithmConfig learningAlgorithmConfig,
            IBackpropagationConfig backpropagationConfig,
            IMLPPropagators propagators,
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
                if (_learningAlgorithmConfig.BatchSize == 1)
                {
                    //online mode
                    //use batch colection variable as a singleton for optimization purposes

                    var batch = new List<IDataItem>(1);
                    batch.Add(null);

                    while (qiterator.MoveNext())
                    {
                        batch[0] = qiterator.Current;

                        ProcessBatch(
                            batch,
                            data.Count,
                            learningRate,
                            ref totalProcessedItemCount
                            );
                    }
                }
                else
                {
                    //batch mode

                    foreach (var batch in qiterator.LazySplit(_learningAlgorithmConfig.BatchSize))
                    {
                        ProcessBatch(
                            batch,
                            data.Count,
                            learningRate,
                            ref totalProcessedItemCount
                            );
                    }
                }
            }

            ConsoleAmbientContext.Console.Write(new string(' ', 60));
            ConsoleAmbientContext.Console.ReturnCarriage();

            //конец эпохи обучения
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ProcessBatch(
            List<IDataItem> batch,
            int dataCount,
            float learningRate,
            ref int totalProcessedItemCount
            )
        {
            var batchProcessedItemCount = 0;
            foreach (var trainDataItem in batch)
            {
                var firstItemInBatch = batchProcessedItemCount == 0;

                #region -> and <- propagate

                _propagators.ForwardPropagator.Propagate(trainDataItem);

                //_desiredValuesContainer.Enqueue(trainDataItem.Output);

                _propagators.Backpropagator.Backpropagate(
                    learningRate,
                    firstItemInBatch
                    );

                #endregion

                #region logging

                const int ItemCountBetweenLogs = 100;

                var logStep = dataCount/ItemCountBetweenLogs;
                if (logStep > 0 && totalProcessedItemCount%logStep == 0)
                {
                    ConsoleAmbientContext.Console.Write(
                        "Epoche progress: {0}%, {1}      ",
                        ((long) totalProcessedItemCount*100/dataCount),
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
}
