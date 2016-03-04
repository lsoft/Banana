using System;
using Banana.MLP.AccuracyRecord;
using Banana.MLP.ArtifactContainer;
using Banana.MLP.Configuration.MLP;
using Banana.MLP.Container.Layer.CSharp;

namespace Banana.MLP.Container.MLP
{
    public interface IMLPContainer
    {
        IMLPConfiguration Configuration
        {
            get;
        }

        void Save(
            IArtifactContainer artifactContainer,
            IAccuracyRecord accuracyRecord
            );

        void InitRandom(
            Random rnd
            );
    }

    public interface IMLPContainer<T> : IMLPContainer
        where T : ILayerContainer
    {

        T[] Layers
        {
            get;
        }

        T LastLayer
        {
            get;
        }

    }
}