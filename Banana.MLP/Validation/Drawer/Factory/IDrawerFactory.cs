using Banana.MLP.ArtifactContainer;

namespace Banana.MLP.Validation.Drawer.Factory
{
    public interface IDrawerFactory
    {
        IDrawer CreateDrawer(
            IArtifactContainer containerForSave,
            int? epocheNumber
            );
    }
}
