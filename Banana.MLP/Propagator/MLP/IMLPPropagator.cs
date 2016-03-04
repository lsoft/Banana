using System;
using System.Collections.Generic;
using Banana.Common.LayerState;
using Banana.Data.Item;
using Banana.MLP.Container.Layer.CSharp;
using Banana.MLP.Container.MLP;
using Banana.MLP.Propagator.Layer;

namespace Banana.MLP.Propagator.MLP
{
    public interface IMLPPropagator
    {
        void Propagate(
            IDataItem item
            );

        List<ILayerState> Propagate(
            IEnumerable<IDataItem> itemList,
            out TimeSpan propagationTime
            );
    }

    public interface IMLPPropagator<T> : IMLPPropagator
        where T : ILayerContainer
    {
        IMLPContainer<T> MLPContainer
        {
            get;
        }

        ILayerPropagator[] ForwardPropagators
        {
            get;
        }

    }
}