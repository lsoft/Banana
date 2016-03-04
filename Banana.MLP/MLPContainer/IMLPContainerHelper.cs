using Banana.MLP.AccuracyRecord;
using Banana.MLP.ArtifactContainer;
using Banana.MLP.Container.Layer.CSharp;
using Banana.MLP.Container.MLP;

namespace Banana.MLP.MLPContainer
{
    public interface IMLPContainerHelper
    {
        T Load<T, U>(
            IArtifactContainer artifactContainer,
            string mlpName
            )
            where T : IMLPContainer<U>
            where U : ILayerContainer;

        void Save<T>(
            IArtifactContainer artifactContainer,
            IMLPContainer<T> mlp,
            IAccuracyRecord accuracyRecord
            )
            where T : ILayerContainer;

    }
}
