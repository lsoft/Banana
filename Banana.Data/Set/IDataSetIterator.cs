using System.Collections.Generic;
using Banana.Data.Item;

namespace Banana.Data.Set
{
    public interface IDataSetIterator : IEnumerator<IDataItem>
    {
        int Count
        {
            get;
        }

    }
}