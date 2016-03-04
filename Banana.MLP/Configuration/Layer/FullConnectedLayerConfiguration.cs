using System;
using Banana.MLP.Dim;
using Banana.MLP.Function;

namespace Banana.MLP.Configuration.Layer
{
    [Serializable]
    public class FullConnectedLayerConfiguration : ILayerConfiguration
    {
        public LayerTypeEnum Type
        {
            get
            {
                return
                    LayerTypeEnum.FullConnected;
            }
        }

        public int TotalNeuronCount
        {
            get;
            private set;
        }

        public int TotalBiasCount
        {
            get;
            private set;
        }

        public IFunction LayerActivationFunction
        {
            get;
            private set;
        }

        public IDimension SpatialDimension
        {
            get;
            private set;
        }

        /// <summary>
        /// Simplest constructor for full-connected MLP
        /// </summary>
        public FullConnectedLayerConfiguration(
            int currentLayerTotalNeuronCount,
            IFunction activationFunction
            ) : this(new Dimension(1, currentLayerTotalNeuronCount), activationFunction)
        {
        }

        /// <summary>
        /// Constructor with custom spatial dimension
        /// </summary>
        public FullConnectedLayerConfiguration(
            IDimension spatialDimension,
            IFunction activationFunction
            ) : this(spatialDimension, activationFunction, spatialDimension.Multiplied)
        {
        }

        /// <summary>
        /// Constructor with custom bias count (it makes sense in case of convolutional neural network)
        /// </summary>
        public FullConnectedLayerConfiguration(
            IDimension spatialDimension,
            IFunction activationFunction,
            int totalBiasCount
            )
        {
            if (spatialDimension == null)
            {
                throw new ArgumentNullException("spatialDimension");
            }
            if (activationFunction == null)
            {
                throw new ArgumentNullException("activationFunction");
            }

            this.SpatialDimension = spatialDimension;
            this.LayerActivationFunction = activationFunction;
            this.TotalNeuronCount = spatialDimension.Multiplied;
            this.TotalBiasCount = totalBiasCount;
        }

        public string GetLayerInformation()
        {
            return
                string.Format(
                    "FC({0} {1})",
                    this.SpatialDimension.GetDimensionInformation(),
                    this.LayerActivationFunction != null
                        ? this.LayerActivationFunction.ShortName
                        : "Input");
        }

    }
}