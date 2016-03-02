using System;
using Banana.MLP.Configuration.Layer;
using OpenCL.Net;
using OpenCL.Net.Wrapper;
using OpenCL.Net.Wrapper.Mem;
using OpenCL.Net.Wrapper.Mem.Data;

namespace Banana.MLP.DesiredValues
{
    public class MemDesiredValuesContainer : IMemDesiredValuesContainer
    {
        public MemFloat DesiredOutput
        {
            get;
            private set;
        }

        public MemDesiredValuesContainer(
            CLProvider clProvider,
            ILayerConfiguration layerConfiguration
            )
        {
            if (clProvider == null)
            {
                throw new ArgumentNullException("clProvider");
            }
            if (layerConfiguration == null)
            {
                throw new ArgumentNullException("layerConfiguration");
            }

            var outputLength = layerConfiguration.TotalNeuronCount;

            this.DesiredOutput = clProvider.CreateFloatMem(
                outputLength,
                MemFlags.CopyHostPtr | MemFlags.ReadOnly
                );
        }

        public void SetValues(float[] desiredValues)
        {
            if (desiredValues == null)
            {
                throw new ArgumentNullException("desiredValues");
            }
            if (desiredValues.Length != DesiredOutput.Array.Length)
            {
                throw new InvalidOperationException("desiredValues.Length != DesiredOutput.Array.Length");
            }

            desiredValues.CopyTo(DesiredOutput.Array, 0);
            DesiredOutput.Write(BlockModeEnum.NonBlocking);
        }
    }
}