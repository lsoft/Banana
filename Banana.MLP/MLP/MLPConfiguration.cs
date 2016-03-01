using System;
using System.Linq;
using Banana.MLP.Layer;

namespace Banana.MLP.MLP
{
    public class MLPConfiguration : IMLPConfiguration
    {
        public string Name
        {
            get;
            private set;
        }

        public ILayerConfiguration[] Layers
        {
            get;
            private set;
        }

        public MLPConfiguration(
            string name,
            ILayerConfiguration[] layers
            )
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (layers == null)
            {
                throw new ArgumentNullException("layers");
            }
            if (layers.Length < 2)
            {
                throw new ArgumentException("layerList.Length < 2");
            }
            if (layers.Any(j => j == null))
            {
                throw new ArgumentException("layerList.Any(j => j == null)");
            }


            Name = name;
            Layers = layers;
        }

        public string GetLayerInformation()
        {
            return
                string.Join(" -> ", this.Layers.ToList().ConvertAll(j => j.GetLayerInformation()));
        }
    }
}