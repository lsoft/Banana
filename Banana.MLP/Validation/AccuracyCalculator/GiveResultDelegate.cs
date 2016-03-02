using Banana.Common.LayerState;
using Banana.Data.Item;

namespace Banana.MLP.Validation.AccuracyCalculator
{
    public delegate void GiveResultDelegate(
        ILayerState modelResult,
        IDataItem origData
        );
}