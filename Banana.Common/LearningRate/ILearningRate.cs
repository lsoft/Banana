namespace Banana.Common.LearningRate
{
    public interface ILearningRate
    {
        float GetLearningRate(int epocheNumber);
    }
}
