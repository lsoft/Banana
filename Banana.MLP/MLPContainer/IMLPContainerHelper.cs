using Banana.MLP.AccuracyRecord;
using Banana.MLP.ArtifactContainer;
using Banana.MLP.Container.MLP;

namespace Banana.MLP.MLPContainer
{
    public interface IMLPContainerHelper
    {
        void Save(
            IArtifactContainer artifactContainer,
            IMLPContainer mlp,
            IAccuracyRecord accuracyRecord
            );

        IMLPContainer Load<T>(
            IArtifactContainer artifactContainer, 
            string mlpName
            ) where T : IMLPContainer;
    }
}
