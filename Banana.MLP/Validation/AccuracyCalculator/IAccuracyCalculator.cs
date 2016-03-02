using Banana.MLP.AccuracyRecord;
using Banana.MLP.Propagator.MLP;
using Banana.MLP.Validation.Drawer;

namespace Banana.MLP.Validation.AccuracyCalculator
{
    public interface IAccuracyCalculator
    {
        void CalculateAccuracy(
            IMLPPropagator forwardPropagation,
            int? epocheNumber,
            IDrawer drawer,
            out IAccuracyRecord accuracyRecord
            );
    }
}
