using System;
using Banana.Data.Item;
using Banana.MLP.Configuration.Layer;

namespace Banana.MLP.Container.Layer.CSharp
{
    public class CSharpLayerContainer : ICSharpLayerContainer
    {
        public ILayerConfiguration Configuration
        {
            get;
            private set;
        }

        public float[] WeightMem
        {
            get;
            private set;
        }

        public float[] BiasMem
        {
            get;
            private set;
        }

        public float[] NetMem
        {
            get;
            private set;
        }

        public float[] StateMem
        {
            get;
            private set;
        }

        public float[] DeDz
        {
            get;
            private set;
        }

        public float[] DeDy
        {
            get;
            private set;
        }

        public float[] NablaWeights
        {
            get;
            private set;
        }

        public float[] NablaBiases
        {
            get;
            private set;
        }

        public CSharpLayerContainer(
            ILayerConfiguration currentLayerConfiguration
            )
        {
            if (currentLayerConfiguration == null)
            {
                throw new ArgumentNullException("currentLayerConfiguration");
            }

            Configuration = currentLayerConfiguration;

            //веса
            WeightMem = null;
            BiasMem = null;

            //нейроны
            NetMem = new float[currentLayerConfiguration.TotalNeuronCount];
            StateMem = new float[currentLayerConfiguration.TotalNeuronCount];

            //производные
            this.DeDz = null;
            this.DeDy = null;

            //дельты
            this.NablaWeights = null;
            this.NablaBiases = null;
        }

        public CSharpLayerContainer(
            ILayerConfiguration previousLayerConfiguration,
            ILayerConfiguration currentLayerConfiguration
            )
        {
            if (previousLayerConfiguration == null)
            {
                throw new ArgumentNullException("previousLayerConfiguration");
            }
            if (currentLayerConfiguration == null)
            {
                throw new ArgumentNullException("currentLayerConfiguration");
            }

            Configuration = currentLayerConfiguration;

            var totalNeuronCount = currentLayerConfiguration.TotalNeuronCount;
            var weightCount = currentLayerConfiguration.TotalNeuronCount * previousLayerConfiguration.TotalNeuronCount;
            var biasCount = currentLayerConfiguration.TotalBiasCount;

            //веса
            WeightMem = new float[weightCount];
            BiasMem = new float[biasCount];

            //нейроны
            NetMem = new float[totalNeuronCount];
            StateMem = new float[totalNeuronCount];

            //производные
            this.DeDz = new float[currentLayerConfiguration.TotalNeuronCount];
            this.DeDy = new float[previousLayerConfiguration.TotalNeuronCount];

            //дельты
            this.NablaWeights = new float[weightCount];
            this.NablaBiases = new float[biasCount];
        }

        /*
        
        public void ClearAndPushNetAndState()
        {
            ClearNetAndState();

            PushNetAndState();
        }

        public void ClearNetAndState()
        {
            var nml = this.NetMem.Length;
            Array.Clear(this.NetMem, 0, nml);

            var sml = this.StateMem.Length;
            Array.Clear(this.StateMem, 0, sml);
        }

        public void PushNetAndState()
        {
            //nothing to do
        }
        //*/

        public void ReadInput(IDataItem data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (data.InputLength != Configuration.TotalNeuronCount)
            {
                throw new ArgumentException("data.InputLength != Configuration.TotalNeuronCount");
            }

            for (var neuronIndex = 0; neuronIndex < Configuration.TotalNeuronCount; neuronIndex++)
            {
                this.NetMem[neuronIndex] = 0; //LastNET
                this.StateMem[neuronIndex] = data.Input[neuronIndex];
            }
        }

        /*
        public void ReadWeightsAndBiasesFromLayer(ILayer layer)
        {
            if (layer == null)
            {
                throw new ArgumentNullException("layer");
            }

            if (this.WeightMem != null || this.BiasMem != null)
            {
                float[] weightMem;
                float[] biasMem;
                layer.GetClonedWeights(
                    out weightMem,
                    out biasMem
                    );

                if (this.WeightMem != null)
                {
                    weightMem.CopyTo(this.WeightMem, 0);
                }

                if (this.BiasMem != null)
                {
                    biasMem.CopyTo(this.BiasMem, 0);
                }
            }
        }

        public void PopNetAndState()
        {
            //nothing to do
        }

        public void PopWeightsAndBiases()
        {
            //nothing to do
        }

        public void WritebackWeightsAndBiasesToMLP(ILayer layer)
        {
            if (this.WeightMem != null && this.BiasMem != null)
            {
                layer.SetWeights(
                    this.WeightMem,
                    this.BiasMem
                    );
            }
        }

        public ILayerState GetLayerState()
        {
            var ls = new LayerState(
                this.StateMem.CloneArray(),
                Configuration.TotalNeuronCount
                );

            return ls;
        }
        
        */

    }
}