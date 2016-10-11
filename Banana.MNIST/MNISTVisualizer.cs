using System;
using System.Drawing;
using System.IO;
using Banana.Common.Others;
using Banana.Common.Visualizer;

namespace Banana.MNIST
{
    public class MNISTVisualizer : IVisualizer
    {
        private const int ImageWidth = 28;
        private const int ImageHeight = 28;

        private readonly Bitmap _gridBitmap;
        private readonly Bitmap _pairBitmap;
        private readonly int _q;

        private int _gridCurrentIndex;
        private int _pairCurrentIndex;

        public MNISTVisualizer(
            int dataCount
            )
        {
            _q = (int)Math.Ceiling(Math.Sqrt(dataCount));
            _gridBitmap = new Bitmap(
                _q * ImageWidth,
                _q * ImageHeight);

            _pairBitmap = new Bitmap(
                ImageWidth * 2 + 1,
                ImageHeight * dataCount);

            _gridCurrentIndex = 0;
            _pairCurrentIndex = 0;
        }

        public void VisualizeGrid(
            float[] data
            )
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            BitmapHelper.CreateContrastEnhancedBitmapFrom(
                data,
                28,
                28, 
                _gridBitmap, 
                (_gridCurrentIndex % _q) * ImageWidth, 
                ((int)(_gridCurrentIndex / _q)) * ImageHeight
                );

            _gridCurrentIndex++;
        }

        public void VisualizePair(
            Pair<float[], float[]> data
            )
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            BitmapHelper.CreateContrastEnhancedBitmapFrom(
                data.First,
                28,
                28, 
                _pairBitmap, 
                0, 
                _pairCurrentIndex * ImageHeight
                );

            BitmapHelper.CreateContrastEnhancedBitmapFrom(
                data.Second,
                28,
                28,
                _pairBitmap, 
                ImageWidth + 1, 
                _pairCurrentIndex * ImageHeight
                );

            _pairCurrentIndex++;
        }

        public void SaveGrid(Stream writeStream)
        {
            _gridBitmap.Save(writeStream, System.Drawing.Imaging.ImageFormat.Bmp);
        }

        public void SavePairs(Stream writeStream)
        {
            _pairBitmap.Save(writeStream, System.Drawing.Imaging.ImageFormat.Bmp);
        }
    }
}