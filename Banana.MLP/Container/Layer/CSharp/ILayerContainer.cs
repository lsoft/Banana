using System;
using Banana.Data.Item;
using Banana.MLP.Configuration.Layer;

namespace Banana.MLP.Container.Layer.CSharp
{
    public interface ILayerContainer
    {
        ILayerConfiguration Configuration
        {
            get;
        }

        void ReadInput(IDataItem data);

        void InitRandom(
            Random rnd
            );
    }
}