using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Banana.Common.IterateHelper;
using Banana.Common.LayerState;
using Banana.Common.Others;
using Banana.Data.Item;
using Banana.Data.Set;
using Banana.MLP.Propagator.MLP;
using Banana.Common.Others;

namespace Banana.MLP.Validation.AccuracyCalculator
{
    public class AccuracyCalculatorBatchIterator
    {
        public void IterateByBatch(
            IDataSet validationData,
            IMLPPropagator forwardPropagation,
            GiveResultDelegate gr
            )
        {
            if (validationData == null)
            {
                throw new ArgumentNullException("validationData");
            }
            if (forwardPropagation == null)
            {
                throw new ArgumentNullException("forwardPropagation");
            }
            if (gr == null)
            {
                throw new ArgumentNullException("gr");
            }

            Task task = null;
            try
            {
                using (IDataSetIterator iterator = validationData.StartIterate())
                {
                    foreach (var validationBatch in iterator.LazySplit(5000))
                    {
                        TimeSpan propagationTime;
                        var netResults = forwardPropagation.Propagate(validationBatch, out propagationTime);

                        if (task != null)
                        {
                            task.Wait();
                            task.Dispose();
                            task = null;
                        }

                        task = new Task(
                            (opair) =>
                            {
                                var pairs = (Pair<List<IDataItem>, List<ILayerState>>) opair;

                                var tValidationItems = pairs.First;
                                var tNetResult = pairs.Second;

                                foreach (var pair in tValidationItems.ZipEqualLength(tNetResult))
                                {
                                    var validationItem = pair.Value1;
                                    var netResult = pair.Value2;

                                    gr(netResult, validationItem);
                                }
                            },
                            new Pair<List<IDataItem>, List<ILayerState>>(validationBatch, netResults)
                            );
                        task.Start();
                    }
                }

                if (task != null)
                {
                    task.Wait();
                }
            }
            finally
            {
                if (task != null)
                {
                    task.Dispose();
                }
            }
        }
    }
}