using System;
using System.Collections.Concurrent;
using System.Threading;
using Banana.Exception;
using Banana.MLP.Configuration.Layer;

namespace Banana.MLP.DesiredValues
{
    public class CSharpDesiredValuesContainer : ICSharpDesiredValuesContainer
    {
        public float[] DesiredOutput
        {
            get;
            private set;
        }

        public CSharpDesiredValuesContainer(
            ILayerConfiguration layerConfiguration
            )
        {
            if (layerConfiguration == null)
            {
                throw new ArgumentNullException("layerConfiguration");
            }

            var outputLength = layerConfiguration.TotalNeuronCount;

            this.DesiredOutput = new float[outputLength];
        }

        public void SetValues(float[] desiredValues)
        {
            if (desiredValues == null)
            {
                throw new ArgumentNullException("desiredValues");
            }
            if (desiredValues.Length != DesiredOutput.Length)
            {
                throw new InvalidOperationException("desiredValues.Length != DesiredOutput.Length");
            }

            desiredValues.CopyTo(DesiredOutput, 0);
        }

    }
}