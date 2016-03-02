using Banana.MLP.Backpropagator.Layer;

namespace Banana.MLP.Backpropagator.MLP
{
    public interface IMLPBackpropagator
    {
        ILayerBackpropagator[] BackPropagators
        {
            get;
        }

        void Backpropagate(
            float learningRate,
            bool firstItemInBatch
            );
    }
}