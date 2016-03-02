using System;
using Banana.Common.Metrics;
using Banana.Common.Others;
using Banana.Data.Set;
using Banana.MLP.AccuracyRecord;
using Banana.MLP.Propagator.MLP;
using Banana.MLP.Validation.Drawer;

namespace Banana.MLP.Validation.AccuracyCalculator
{
    public class MetricsAccuracyCalculator : IAccuracyCalculator
    {
        private readonly IMetrics _metrics;
        private readonly IDataSet _validationData;

        private readonly AccuracyCalculatorBatchIterator _batchIterator;

        public MetricsAccuracyCalculator(
            IMetrics metrics,
            IDataSet validationData
            )
        {
            if (metrics == null)
            {
                throw new ArgumentNullException("metrics");
            }
            if (validationData == null)
            {
                throw new ArgumentNullException("validationData");
            }

            _metrics = metrics;
            _validationData = validationData;

            _batchIterator = new AccuracyCalculatorBatchIterator();
        }

        public void CalculateAccuracy(
            IMLPPropagator forwardPropagation,
            int? epocheNumber,
            IDrawer drawer,
            out IAccuracyRecord accuracyRecord
            )
        {
            if (forwardPropagation == null)
            {
                throw new ArgumentNullException("forwardPropagation");
            }
            //drawer allowed to be null

            if (drawer != null)
            {
                drawer.SetSize(
                    _validationData.Count
                    );
            }

            var totalErrorAcc = new KahanAlgorithm.Accumulator();

            _batchIterator.IterateByBatch(
                _validationData,
                forwardPropagation,
                (netResult, testItem) =>
                {
                    #region рисуем итем

                    if (drawer != null)
                    {
                        drawer.DrawItem(netResult);
                    }

                    #endregion

                    #region суммируем ошибку

                    var err = _metrics.Calculate(
                        testItem.Output,
                        netResult.NState
                        );

                    totalErrorAcc.Add(err);

                    #endregion
                });

            var totalError = totalErrorAcc.Sum;

            var perItemError = totalError / _validationData.Count;

            accuracyRecord = new MetricAccuracyRecord(
                epocheNumber ?? 0,
                perItemError);

            if (drawer != null)
            {
                drawer.Save();
            }

        }

    }
}