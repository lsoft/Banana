using System;
using Banana.MLP.AccuracyRecord;
using Banana.MLP.ArtifactContainer;
using Banana.MLP.Container.MLP;

namespace Banana.MLP.MLPContainer
{
    public class MLPContainerHelper : IMLPContainerHelper
    {
        public MLPContainerHelper(
            )
        {
        }

        public IMLPContainer Load<T>(
            IArtifactContainer artifactContainer,
            string mlpName
            )
            where T : IMLPContainer
        {
            if (artifactContainer == null)
            {
                throw new ArgumentNullException("artifactContainer");
            }

            var result = artifactContainer.LoadSerialized<T>(mlpName);

            return result;
        }


        public void Save(
            IArtifactContainer artifactContainer,
            IMLPContainer mlp,
            IAccuracyRecord accuracyRecord
            )
        {
            if (artifactContainer == null)
            {
                throw new ArgumentNullException("artifactContainer");
            }
            if (mlp == null)
            {
                throw new ArgumentNullException("mlp");
            }
            if (accuracyRecord == null)
            {
                throw new ArgumentNullException("accuracyRecord");
            }

            //сохраняем сеть
            artifactContainer.SaveSerialized(
                mlp,
                "mlpcontainer.bin"
                );
            
            //сохраняем файл с результатами
            artifactContainer.SaveString(
                accuracyRecord.GetTextResults(),
                "_result.txt"
                );
        }

    }
}