using System;
using System.Collections.Generic;
using System.Linq;
using Banana.Data.Item;
using Banana.Data.Iterator;
using Banana.Exception;

namespace Banana.Data.Set
{
    [Serializable]
    public class InMemoryDataSet : IDataSet
    {
        private readonly IList<IDataItem> _dataList;

        public int Count
        {
            get
            {
                return
                    _dataList.Count;
            }
        }

        public bool IsAutoencoderDataSet
        {
            get;
            private set;
        }

        public int InputLength
        {
            get
            {
                return
                    _dataList[0].InputLength;
            }
        }

        public int OutputLength
        {
            get
            {
                return
                    _dataList[0].OutputLength;
            }
        }

        public InMemoryDataSet(
            IList<IDataItem> dataList
            )
        {
            if (dataList == null)
            {
                throw new ArgumentNullException("dataList");
            }
            if (dataList.Count == 0)
            {
                throw new BananaException(
                    "Incoming data list is empty",
                    BananaErrorEnum.DataError
                    );
            }
            if (_dataList.Select(j => j.Input).Distinct().Count() != 1)
            {
                throw new BananaException(
                    "Incoming data list has no consistency in input array.",
                    BananaErrorEnum.DataError
                    );
            }
            if (_dataList.Select(j => j.Output).Distinct().Count() != 1)
            {
                throw new BananaException(
                    "Incoming data list has no consistency in output array.",
                    BananaErrorEnum.DataError
                    );
            }

            _dataList = dataList;

            this.IsAutoencoderDataSet = _dataList[0].InputLength == _dataList[0].OutputLength);
        }

        public IDataSetIterator StartIterate()
        {
            return
                new InMemoryDataSetIterator(_dataList);
        }

    }
}