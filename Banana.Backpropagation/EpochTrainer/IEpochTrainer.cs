using Banana.Data.Set;

namespace Banana.Backpropagation.EpochTrainer
{
    public interface IEpochTrainer
    {
        void Train(
            IDataSet data,
            float learningRate
            );
    }
}