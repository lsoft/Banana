namespace Banana.MLP.DesiredValues
{
    public interface ICSharpDesiredValuesContainer : IDesiredValuesContainer
    {
        float[] DesiredOutput
        {
            get;
        }

    }
}