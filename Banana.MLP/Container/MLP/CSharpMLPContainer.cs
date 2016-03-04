using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banana.MLP.Configuration.MLP;
using Banana.MLP.Container.Layer.CSharp;

namespace Banana.MLP.Container.MLP
{
    public class CSharpMLPContainer : IMLPContainer<CSharpLayerContainer>
    {
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
            IMLPConfiguration configuration
            )
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
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
    }
}
