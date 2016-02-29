﻿namespace Banana.Data.Item
{
    public interface IDataItem
    {
        int InputLength
        {
            get;
        }

        int OutputLength
        {
            get;
        }

        int OutputIndex
        {
            get;
        }

        float[] Input
        {
            get;
        }

        float[] Output
        {
            get;
        }
    }
}