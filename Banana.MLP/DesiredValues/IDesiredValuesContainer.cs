namespace Banana.MLP.DesiredValues
{
    public interface IDesiredValuesContainer
    {
        void Enqueue(float[] desiredValues);

        bool MoveNext();

        void Reset();
    }
}