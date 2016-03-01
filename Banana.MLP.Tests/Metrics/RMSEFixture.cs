using Banana.Common.Metrics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Banana.MLP.Tests.Metrics
{
    [TestClass]
    public class RMSEFixture
    {
        [TestMethod]
        public void Test()
        {
            var metric = new RMSE();

            var mt = new MetricTester();
            mt.Test(
                metric
                );
        }
    }
}