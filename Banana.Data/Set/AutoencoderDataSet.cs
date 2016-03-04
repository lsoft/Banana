using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banana.Common.Others;
using Banana.Data.Item;

namespace Banana.Data.Set
{
    public class AutoencoderDataSet : IDataSet
    {
        private readonly IDataSet _dataSet;
        private readonly IDataItemFactory _dataItemFactory;

        public int Count
        {
            get
            {
                return
                    _dataSet.Count;
            }
        }

        public bool IsAutoencoderDataSet
        {
            get
            {
                return
                    true;
            }
        }

        public int InputLength
        {
            get
            {
                return
                    _dataSet.InputLength;
            }
        }

        public int OutputLength
        {
            get
            {
                return
                    _dataSet.InputLength; //it's AUTOENCODER data set
            }
        }


        public AutoencoderDataSet(
            IDataSet dataSet,
            IDataItemFactory dataItemFactory
            )
        {
            if (dataSet == null)
            {
                throw new ArgumentNullException("dataSet");
            }
            if (dataItemFactory == null)
            {
                throw new ArgumentNullException("dataItemFactory");
            }

            _dataSet = dataSet;
            _dataItemFactory = dataItemFactory;
        }

        public IDataSetIterator StartIterate()
        {
            return
                new ToAutoencoderDataSetIterator(
                    _dataSet,
                    _dataItemFactory
                    );
        }

        private class ToAutoencoderDataSetIterator : IDataSetIterator
        {
            private readonly IDataSet _dataSet;
            private readonly IDataItemFactory _dataItemFactory;

            private readonly IDataSetIterator _iterator;

            public IDataItem Current
            {
                get;
                private set;
            }

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            public int Count
            {
                get
                {
                    return
                        _dataSet.Count;
                }
            }

            public ToAutoencoderDataSetIterator(
                IDataSet dataSet,
                IDataItemFactory dataItemFactory
                )
            {
                if (dataSet == null)
                {
                    throw new ArgumentNullException("dataSet");
                }
                if (dataItemFactory == null)
                {
                    throw new ArgumentNullException("dataItemFactory");
                }

                _dataSet = dataSet;
                _dataItemFactory = dataItemFactory;

                _iterator = dataSet.StartIterate();
            }

            public bool MoveNext()
            {
                var result = _iterator.MoveNext();

                if (result)
                {
                    var item = _iterator.Current;

                    var convertedItem = _dataItemFactory.CreateDataItem(
                        item.Input,
                        item.Input.CloneArray()
                        );

                    this.Current = convertedItem;
                }
                else
                {
                    this.Current = null;
                }

                return
                    result;
            }

            public void Reset()
            {
                _iterator.Reset();
                
                Current = null;
            }

            public void Dispose()
            {
                //nothing to do
            }

        }
    }
}
