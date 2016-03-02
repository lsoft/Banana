using Banana.MLP.Dim;
using Banana.MLP.Function;

namespace Banana.MLP.Configuration.Layer
{
    public interface ILayerConfiguration
    {
        /// <summary>
        /// Layer type
        /// </summary>
        LayerTypeEnum Type
        {
            get;
        }

        /// <summary>
        /// Total layer neuron count
        /// </summary>
        int TotalNeuronCount
        {
            get;
        }

        /// <summary>
        /// Total layer bias count
        /// </summary>
        int TotalBiasCount
        {
            get;
        }

        /// <summary>
        /// Layer activation function
        /// </summary>
        IFunction LayerActivationFunction
        {
            get;
        }

        string GetLayerInformation();

        /// <summary>
        /// Layer spatial dimensions
        /// </summary>
        IDimension SpatialDimension
        {
            get;
        }

    }
}