using Banana.Common.Metrics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Banana.MLP.Tests.Metrics
{
    [TestClass]
    public class LogLikelihoodFixture
    {
        [TestMethod]
        public void Test()
        {
            var metric = new Loglikelihood();

            var mt = new MetricTester();
            mt.Test(
                metric,
                (random) => (float) (random.NextDouble() * 0.7f + 0.1f)
                );
        }
    }
}