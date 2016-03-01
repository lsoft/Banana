using System;
using Banana.MLP.Layer;

namespace Banana.MLP.Container.CSharp
{
    public interface ICSharpLayerContainer
    {
        ILayerConfiguration Configuration
        {
            get;
        }

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
    }
}