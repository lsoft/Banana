using System;
using System.Drawing;
using System.Linq;

namespace Banana.Common.Visualizer
{
    public class BitmapHelper
    {
        public static void CreateContrastEnhancedBitmapFrom(
            float[] layer, 
            int width, 
            int height, 
            Bitmap bitmap, 
            int left, 
            int top
            )
        {
            if (bitmap == null)
            {
                throw new ArgumentNullException("bitmap");
            }
            if (layer == null)
            {
                throw new ArgumentNullException("layer");
            }
            if (layer.Length < width*height)
            {
                throw new ArgumentException("layer.Length < width*height");
            }

            float max = layer.Take(width*height).Max(val => val);
            float min = layer.Take(width*height).Min(val => val);

            if (Math.Abs(min - max) <= float.Epsilon)
            {
                min = 0;
                max = 1;
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float value = layer[PointToIndex(x, y, width)];
                    value = (value - min)/(max - min);
                    var b = (byte) Math.Max(0, Math.Min(255, value*255.0));

                    bitmap.SetPixel(left + x, top + y, Color.FromArgb(b, b, b));
                }
            }
        }

        private static int PointToIndex(int x, int y, int width)
        {
            return y*width + x;
        }
    }
}