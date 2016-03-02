namespace Banana.Common.Visualizer.Factory
{
    public interface IVisualizerFactory
    {
        IVisualizer CreateVisualizer(
            int dataCount
            );
    }
}
