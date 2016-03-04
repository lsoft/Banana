using System;
using Banana.MLP.Backpropagator.Layer;
using Banana.MLP.Classic.BackPropagation.DeDyAggregator;
using Banana.MLP.Classic.BackPropagation.DeDzCalculator;
using Banana.MLP.Classic.BackPropagation.UpdateNablaExecutor;

namespace Banana.MLP.Classic.BackPropagation.Backpropagator.Layer
{
    public class CSharpLayerBackpropagator : ILayerBackpropagator
    {
        private readonly IDeDyAggregator _deDyAggregator;
        private readonly IDeDzCalculator _deDzCalculator;
        private readonly IUpdateNablaExecutor _updateNablaExecutor;

        public CSharpLayerBackpropagator(
            IDeDyAggregator deDyAggregator,
            IDeDzCalculator deDzCalculator,
            IUpdateNablaExecutor updateNablaExecutor
            )
        {
            if (deDyAggregator == null)
            {
                throw new ArgumentNullException("deDyAggregator");
            }
            if (deDzCalculator == null)
            {
                throw new ArgumentNullException("deDzCalculator");
            }
            if (updateNablaExecutor == null)
            {
                throw new ArgumentNullException("updateNablaExecutor");
            }

            _deDyAggregator = deDyAggregator;
            _deDzCalculator = deDzCalculator;
            _updateNablaExecutor = updateNablaExecutor;
        }

        public void Backpropagate(
            float learningRate,
            bool firstItemInBatch
            )
        {
            _deDzCalculator.Calculate();

            if (firstItemInBatch)
            {
                _updateNablaExecutor.CalculateOverwrite(learningRate);
            }
            else
            {
                _updateNablaExecutor.CalculateIncrement(learningRate);
            }

            _deDyAggregator.Aggregate();
        }
    }
}
