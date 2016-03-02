namespace Banana.MLP.Container.Layer.CSharp
{
    public interface ICSharpLayerContainer : ILayerContainer
    {
        float[] WeightMem
        {
            get;
        }

        float[] BiasMem
        {
            get;
        }

        float[] NetMem
        {
            get;
        }

        float[] StateMem
        {
            get;
        }

        float[] DeDz
        {
            get;
        }

        float[] DeDy
        {
            get;
        }

        float[] NablaWeights
        {
            get;
        }

        float[] NablaBiases
        {
            get;
        }

    }
}