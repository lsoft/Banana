﻿using System.IO;

namespace Banana.MLP.ArtifactContainer
{
    public interface IArtifactContainer : IArtifactReadContainer
    {
        IArtifactContainer Parent
        {
            get;
        }

        void SaveString(string message, string resourceName);

        void SaveSerialized<T>(T obj, string resourceName);

        Stream GetWriteStreamForResource(string resourceName);

        IArtifactContainer GetChildContainer(string containerName);
        
        void Clear();

        void DeleteResource(string resourceName);
    }
}
