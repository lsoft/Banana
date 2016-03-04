using System.Text;
using Banana.Common.Visualizer;
using Banana.Common.Visualizer.Factory;
using Banana.Data.Normalizer;

namespace Banana.MNIST
{
    public class MNISTVisualizerFactory : IVisualizerFactory
    {
        public IVisualizer CreateVisualizer(
            int dataCount
            )
        {
            return
                new MNISTVisualizer(dataCount);

        }
    }
}
