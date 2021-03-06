﻿using System;
using Banana.Common.Others;

namespace Banana.MLP.Dim
{
    [Serializable]
    public class Dimension : IDimension
    {
        public int DimensionCount
        {
            get;
            private set;
        }

        public int[] Sizes
        {
            get;
            private set;
        }

        public int LastDimensionSize
        {
            get
            {
                return
                    Sizes[Sizes.Length - 1];
            }
        }

        public int Width
        {
            get
            {
                return
                    Sizes[0];
            }
        }

        public int Height
        {
            get
            {
                return
                    Sizes[1];
            }
        }

        public int Multiplied
        {
            get
            {
                return
                    Sizes.Mul();
            }
        }

        public Dimension(
            int dimensionCount,
            params int[] sizes
            )
        {
            if (sizes == null)
            {
                throw new ArgumentNullException("sizes");
            }
            if (dimensionCount != sizes.Length)
            {
                throw new ArgumentException("dimensionCount != sizes.Length");
            }

            DimensionCount = dimensionCount;
            Sizes = sizes;
        }

        public string GetDimensionInformation(
            )
        {
            return
                string.Join(
                    "x",
                    this.Sizes);
        }

        public bool IsEqual(
            IDimension dim
            )
        {
            if (dim == null)
            {
                return false;
            }

            if (this.DimensionCount != dim.DimensionCount)
            {
                return false;
            }

            if (!ArrayHelper.ValuesAreEqual(this.Sizes, dim.Sizes))
            {
                return false;
            }

            return true;
        }

        public IDimension Rescale(float scaleFactor)
        {
            return
                new Dimension(
                    this.DimensionCount,
                    this.Sizes.ConvertAll(j => (int)(j * scaleFactor))
                    );
        }
    }

}
