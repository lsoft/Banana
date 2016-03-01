namespace Banana.MLP.Classic.ForwardPropagation
{
    public interface ILayerPropagator
    {
        void ComputeLayer(
            );

        void WaitForCalculationFinished();
    }
}