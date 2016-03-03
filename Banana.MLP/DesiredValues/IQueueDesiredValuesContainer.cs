namespace Banana.MLP.DesiredValues
{
    //public interface IQueueDesiredValuesContainerFactory
    //{
    //    IQueueDesiredValuesContainer Create();
    //}

    public interface IQueueDesiredValuesContainer
    {
        void SetValues(float[] desiredValues);

        bool MoveNext();

        void Reset();
    }
}