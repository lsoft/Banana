using Banana.Common.Metrics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Banana.MLP.Tests.Metrics
{
    [TestClass]
    public class HalfSquaredEuclidianDistanceFixture
    {
        [TestMethod]
        public void Test()
        {
            var metric = new HalfSquaredEuclidianDistance();

            var mt = new MetricTester();
            mt.Test(
                metric
                );
        }
    }
}
