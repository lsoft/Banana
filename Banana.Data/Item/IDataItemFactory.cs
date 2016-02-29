namespace Banana.Data.Item
{
    public interface IDataItemFactory
    {
        IDataItem CreateDataItem(
            float[] input,
            float[] output
            );
    }
}