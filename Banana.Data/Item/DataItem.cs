﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banana.Exception;

namespace Banana.Data.Item
{
    [Serializable]
    public class DataItem : IDataItem
    {
        private readonly float[] _input;
        private readonly float[] _output;

        public float[] Input
        {
            get
            {
                return _input;
            }
        }

        public float[] Output
        {
            get
            {
                return _output;
            }
        }

        public int InputLength
        {
            get
            {
                return
                    _input.Length;
            }
        }

        public int OutputLength
        {
            get
            {
                return
                    _output.Length;
            }
        }

        public int OutputIndex
        {
            get
            {
                var result = -1;

                for (var cc = 0; cc < _output.Length; cc++)
                {
                    if (_output[cc] >= float.Epsilon)
                    {
                        if (result >= 0)
                        {
                            throw new BananaException(
                                "The data item is not a classification item",
                                BananaErrorEnum.DataError
                                );
                        }

                        result = cc;
                    }
                }

                return result;
            }
        }

        private DataItem()
        {
        }

        internal DataItem(
            float[] input,
            float[] output
            )
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }

            _input = input;
            _output = output;
        }

    }
}
