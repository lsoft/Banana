using Banana.MLP.Container.Layer.CSharp;

namespace Banana.MLP.Propagator.Layer
{
    public interface ILayerPropagator
    {
        ILayerContainer LayerContainer
        {
            get;
        }

        void ComputeLayer(
            );

        void WaitForCalculationFinished();
    }
}