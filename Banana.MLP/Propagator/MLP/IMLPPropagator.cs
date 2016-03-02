using System;
using System.Collections.Generic;
using Banana.Common.LayerState;
using Banana.Data.Item;
using Banana.MLP.Container.MLP;
using Banana.MLP.Propagator.Layer;

namespace Banana.MLP.Propagator.MLP
{
    public interface IMLPPropagator
    {
        IMLPContainer MLPContainer
        {
            get;
        }

        ILayerPropagator[] ForwardPropagators
        {
            get;
        }

        void Propagate(
            IDataItem item
            );

        List<ILayerState> Propagate(
            IEnumerable<IDataItem> itemList,
            out TimeSpan propagationTime
            );
    }
}