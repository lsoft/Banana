namespace Banana.MLP.ArtifactContainer
{
    public interface IArtifactReadContainer
    {
        T DeepClone<T>(
            T obj
            );

        T LoadSerialized<T>(string resourceName);
    }
}