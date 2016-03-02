using Banana.MLP.Classic.BackPropagation.UpdateNeuronExecutor;

namespace Banana.Backpropagation.UpdateNeuronExecutors
{
    public interface IUpdateNeuronExecutors
    {
        IUpdateNeuronExecutor[] UpdateNeuronExecutors
        {
            get;
        }

        void Execute(
            );
    }
}