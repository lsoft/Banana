﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banana.MLP.AccuracyRecord;
using Banana.MLP.ArtifactContainer;
using Banana.MLP.Configuration.MLP;
using Banana.MLP.Container.Layer.CSharp;
using Banana.MLP.MLPContainer;

namespace Banana.MLP.Container.MLP
{
    public class CSharpMLPContainer : IMLPContainer<CSharpLayerContainer>
    {
        private readonly IMLPContainerHelper _mlpContainerHelper;

        public IMLPConfiguration Configuration
        {
            get;
            private set;
        }

        public CSharpLayerContainer[] Layers
        {
            get;
            private set;
        }

        public CSharpMLPContainer(
            IMLPConfiguration configuration,
            IMLPContainerHelper mlpContainerHelper
            )
        {
            _mlpContainerHelper = mlpContainerHelper;
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }
            if (mlpContainerHelper == null)
            {
                throw new ArgumentNullException("mlpContainerHelper");
            }

            Configuration = configuration;

            Layers = new CSharpLayerContainer[configuration.Layers.Length];

            Layers[0] = new CSharpLayerContainer(configuration.Layers[0]);

            for (var cc = 1; cc < configuration.Layers.Length; cc++)
            {
                Layers[cc] = new CSharpLayerContainer(
                    configuration.Layers[cc - 1],
                    configuration.Layers[cc]
                    );
            }
        }

        public void Save(
            IArtifactContainer artifactContainer,
            IAccuracyRecord accuracyRecord
            )
        {
            if (artifactContainer == null)
            {
                throw new ArgumentNullException("artifactContainer");
            }
            if (accuracyRecord == null)
            {
                throw new ArgumentNullException("accuracyRecord");
            }

            _mlpContainerHelper.Save(
                artifactContainer,
                this,
                accuracyRecord
                );
        }
    }
}
