using Banana.MLP.DesiredValues;

namespace Banana.MLP.Classic.BackPropagation.UpdateNablaExecutor
{
    public interface IUpdateNablaExecutor
    {
        void CalculateOverwrite(
            float learningRate
            );

        void CalculateIncrement(
            float learningRate
            );
    }
}