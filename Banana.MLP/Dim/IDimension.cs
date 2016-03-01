namespace Banana.MLP.Dim
{
    public interface IDimension
    {
        int DimensionCount
        {
            get;
        }

        int[] Sizes
        {
            get;
        }

        int LastDimensionSize
        {
            get;
        }

        int Width
        {
            get;
        }

        int Height
        {
            get;
        }

        int Multiplied
        {
            get;
        }

        string GetDimensionInformation(
            );

        bool IsEqual(
            IDimension dim
            );

        IDimension Rescale(float scaleFactor);
    }
}