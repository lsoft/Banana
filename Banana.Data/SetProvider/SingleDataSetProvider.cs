using System;
using Banana.Data.Set;

namespace Banana.Data.SetProvider
{
    [Serializable]
    public class SingleDataSetProvider : IDataSetProvider
    {
        private readonly IDataSet _dataSet;

        public int Count
        {
            get
            {
                return
                    _dataSet.Count;
            }
        }

        public SingleDataSetProvider(
            IDataSet dataSet
            )
        {
            if (dataSet == null)
            {
                throw new ArgumentNullException("dataSet");
            }
            _dataSet = dataSet;
        }

        public IDataSet GetDataSet(int epochNumber)
        {
            return
                _dataSet;
        }
    }
}