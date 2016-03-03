namespace Banana.MLP.DesiredValues
{
    public interface IDesiredValuesContainer
    {
        void SetValues(float[] desiredValues);

        bool MoveNext();

        void Reset();
    }
}