using Banana.Common.LayerState;

namespace Banana.MLP.Validation.Drawer
{
    public interface IDrawer
    {
        void SetSize(
            int netResultCount
            );

        void DrawItem(
            ILayerState netResult
            );

        void Save();
    }
}