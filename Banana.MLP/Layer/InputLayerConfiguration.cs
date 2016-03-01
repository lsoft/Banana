using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banana.MLP.Dim;
using Banana.MLP.Function;

namespace Banana.MLP.Layer
{
    public class InputLayerConfiguration : ILayerConfiguration
    {
        public LayerTypeEnum Type
        {
            get
            {
                return
                    LayerTypeEnum.Input;
            }
        }

        public int TotalNeuronCount
        {
            get;
            private set;
        }

        public int TotalBiasCount
        {
            get
            {
                return
                    0;
            }
        }

        public IFunction LayerActivationFunction
        {
            get
            {
                return
                    FakeInputFunction.FakeSingleton;
            }
        }

        public IDimension SpatialDimension
        {
            get;
            private set;
        }

        public InputLayerConfiguration(
            int currentLayerTotalNeuronCount
            )
            : this(new Dimension(1, currentLayerTotalNeuronCount))
        {
        }

        public InputLayerConfiguration(
            IDimension dimension
            )
        {
            TotalNeuronCount = dimension.Multiplied;
            SpatialDimension = dimension;
        }


        public string GetLayerInformation()
        {
            return
                string.Format(
                    "FC({0} {1})",
                    this.SpatialDimension.GetDimensionInformation(),
                    this.LayerActivationFunction.ShortName
                    );
        }
    }
}
