namespace Banana.MLP.Backpropagator.Layer
{
    public interface ILayerBackpropagator
    {
        void Backpropagate(
            float learningRate,
            bool firstItemInBatch
            );
    }
}