using System.Diagnostics;
using System.Globalization;
using Banana.Common;
using Banana.Common.Others;
using Banana.MLP.Classic.BackPropagation.DeDyAggregator;
using Banana.MLP.Configuration.Layer;
using Banana.MLP.Container.Layer.CSharp;
using Banana.MLP.Function;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Banana.MLP.Tests.DeDyAggregator.CSharp
{
    [TestClass]
    public class CSharpDeDyAggregatorFixture
    {
        [TestMethod]
        public void TestDeDyVersusDeDy2()
        {
            const int PreviousLayerTotalNeuronCount = 800;
            const int AggregateLayerTotalNeuronCount = 700;

            var previousLayerConfiguration = new FullConnectedLayerConfiguration(
                PreviousLayerTotalNeuronCount,
                new LinearFunction(1f)
                );

            var aggregateLayerConfiguration = new FullConnectedLayerConfiguration(
                AggregateLayerTotalNeuronCount,
                new LinearFunction(1f)
                );

            var aggregateLayer = new CSharpLayerContainer(
                previousLayerConfiguration,
                aggregateLayerConfiguration
                );

            var dedy = new CSharpDeDyAggregator(
                previousLayerConfiguration,
                aggregateLayer
                );
            var dedy2 = new CSharpDeDyAggregator2(
                previousLayerConfiguration,
                aggregateLayer
                );

            aggregateLayer.DeDz.Fill((i) => RandomHelper.GetRandomInt(15));
            aggregateLayer.WeightMem.Fill((i) => RandomHelper.GetRandomInt(15));
            aggregateLayer.DeDy.Clear();

            #region naive DeDy aggregator run

            using (new PerformanceTimerHelper(secondsTaken => Debug.WriteLine("DeDy  taken: {0}", secondsTaken)))
            {
                dedy.Aggregate();
            }

            var deDyResult = aggregateLayer.DeDy.CloneArray();

            #endregion

            aggregateLayer.DeDy.Clear();

            #region optimized DeDy aggregator run

            using (new PerformanceTimerHelper(secondsTaken => Debug.WriteLine("DeDy2 taken: {0}", secondsTaken)))
            {
                dedy2.Aggregate();
            }

            //dedy2.Aggregate();

            var deDy2Result = aggregateLayer.DeDy.CloneArray();

            #endregion

            int maxDiffIndex;
            float maxDiff;
            var equals = ArrayHelper.ValuesAreEqual(deDyResult, deDy2Result, float.Epsilon, out maxDiff, out maxDiffIndex);

            Debug.WriteLine(
                "{0}  {1}  {2}",
                equals ? "EQUALS" : "NOT EQUALS",
                equals ? string.Empty : maxDiffIndex.ToString(),
                equals ? string.Empty : maxDiff.ToString(CultureInfo.InvariantCulture)
                );

            deDyResult.DebugDump("DeDy : {0}", 64);
            deDy2Result.DebugDump("DeDy2: {0}", 64);

            Assert.IsTrue(equals);
        }

        [TestMethod]
        public void TestDeDy2VersusDeDy2Again()
        {
            const int PreviousLayerTotalNeuronCount = 800;
            const int AggregateLayerTotalNeuronCount = 700;

            var previousLayerConfiguration = new FullConnectedLayerConfiguration(
                PreviousLayerTotalNeuronCount,
                new LinearFunction(1f)
                );

            var aggregateLayerConfiguration = new FullConnectedLayerConfiguration(
                AggregateLayerTotalNeuronCount,
                new LinearFunction(1f)
                );

            var aggregateLayer = new CSharpLayerContainer(
                previousLayerConfiguration,
                aggregateLayerConfiguration
                );

            var dedy2 = new CSharpDeDyAggregator2(
                previousLayerConfiguration,
                aggregateLayer
                );

            aggregateLayer.DeDz.Fill((i) => RandomHelper.GetRandomInt(15));
            aggregateLayer.WeightMem.Fill((i) => RandomHelper.GetRandomInt(15));
            aggregateLayer.DeDy.Clear();

            #region first DeDy aggregator run

            using (new PerformanceTimerHelper(secondsTaken => Debug.WriteLine("DeDy2 at first  taken: {0}", secondsTaken)))
            {
                dedy2.Aggregate();
            }

            var deDy2Result0 = aggregateLayer.DeDy.CloneArray();

            #endregion

            //DO NOT clearing the aggregateLayer.DeDy
            //DeDy aggregator should do it itself!

            #region second DeDy aggregator run

            using (new PerformanceTimerHelper(secondsTaken => Debug.WriteLine("DeDy2 at second taken: {0}", secondsTaken)))
            {
                dedy2.Aggregate();
            }

            //dedy2.Aggregate();

            var deDy2Result1 = aggregateLayer.DeDy.CloneArray();

            #endregion

            int maxDiffIndex;
            float maxDiff;
            var equals = ArrayHelper.ValuesAreEqual(deDy2Result0, deDy2Result1, float.Epsilon, out maxDiff, out maxDiffIndex);

            Debug.WriteLine(
                "{0}  {1}  {2}",
                equals ? "EQUALS" : "NOT EQUALS",
                equals ? string.Empty : maxDiffIndex.ToString(),
                equals ? string.Empty : maxDiff.ToString(CultureInfo.InvariantCulture)
                );

            deDy2Result0.DebugDump("DeDy2 at first : {0}", 64);
            deDy2Result1.DebugDump("DeDy2 at second: {0}", 64);

            Assert.IsTrue(equals);
        }
    }
}
