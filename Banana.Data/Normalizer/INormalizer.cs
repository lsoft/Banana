namespace Banana.Data.Normalizer
{
    public interface INormalizer
    {
        /// <summary>
        /// Linear normalization into [0;1]
        /// </summary>
        void Normalize(
            float[] dataToNormalize,
            float bias = 0f
            );

        /// <summary>
        /// Gauss normalization with mean = 0, variance = 1, standard deviation = 1
        /// </summary>
        void GNormalize(
            float[] dataToNormalize
            );
    }
}