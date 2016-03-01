using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banana.Common.LearningRate;
using Banana.Common.Metrics;

namespace Banana.MLP.LearningConfig
{
    public interface ILearningAlgorithmConfig
    {
        /// <summary>
        /// Метрика, на которую обучается MLP
        /// </summary>
        IMetrics TargetMetrics
        {
            get;
        }

        /// <summary>
        /// Size of the butch. -1 means fullbatch size. 
        /// </summary>
        int BatchSize
        {
            get;
        }

        float RegularizationFactor
        {
            get;
        }

        int MaxEpoches
        {
            get;
        }
    }
}
