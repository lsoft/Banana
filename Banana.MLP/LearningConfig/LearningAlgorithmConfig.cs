using System;
using Banana.Common.Metrics;

namespace Banana.MLP.LearningConfig
{
    public class LearningAlgorithmConfig : ILearningAlgorithmConfig
    {
        /// <summary>
        /// Метрика, на которую обучается MLP
        /// </summary>
        public IMetrics TargetMetrics
        {
            get;
            private set;
        }

        /// <summary>
        /// Size of the butch. -1 means fullbatch size. 
        /// </summary>
        public int BatchSize
        {
            get;
            private set;
        }

        public float RegularizationFactor
        {
            get;
            private set;
        }

        public int MaxEpoches
        {
            get;
            private set;
        }


        /// <summary>
        /// Для сериализатора
        /// </summary>
        public LearningAlgorithmConfig()
        {
        }

        public LearningAlgorithmConfig(
            IMetrics targetMetrics,
            int batchSize,
            float regularizationFactor,
            int maxEpoches
            )
        {
            if (targetMetrics == null)
            {
                throw new ArgumentNullException("targetMetrics");
            }

            TargetMetrics = targetMetrics;
            BatchSize = batchSize;
            RegularizationFactor = regularizationFactor;
            MaxEpoches = maxEpoches;
        }

    }
}