using Banana.Backpropagation.UpdateNeuronExecutors;
using Banana.MLP.Backpropagator.MLP;
using Banana.MLP.Classic.BackPropagation.Backpropagator;
using Banana.MLP.Classic.BackPropagation.Backpropagator.MLP;
using Banana.MLP.Classic.BackPropagation.UpdateNeuronExecutor;
using Banana.MLP.Classic.ForwardPropagation;
using Banana.MLP.Classic.ForwardPropagation.MLP;
using Banana.MLP.Container.Layer.CSharp;
using Banana.MLP.Container.MLP;
using Banana.MLP.Propagator.MLP;

namespace Banana.Backpropagation.Propagators
{
    public interface IMLPPropagators<T>
        where T : ILayerContainer
    {
        IMLPContainer<T> MLPContainer
        {
            get;
        }

        IMLPPropagator<T> ForwardPropagator
        {
            get;
        }

        IMLPBackpropagator Backpropagator
        {
            get;
        }

        IUpdateNeuronExecutors UpdateNeuronExecutor
        {
            get;
        }
    }
}