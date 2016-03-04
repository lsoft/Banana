using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banana.Common.Others;

namespace Banana.Data.Normalizer
{
    [Serializable]
    public class DefaultNormalizer : INormalizer
    {
        public void Normalize(
            float[] dataToNormalize,
            float bias = 0f
            )
        {
            if (dataToNormalize == null)
            {
                throw new ArgumentNullException("dataToNormalize");
            }
            var min = dataToNormalize.Min();
            var max = dataToNormalize.Max();

            for (var dd = 0; dd < dataToNormalize.Length; dd++)
            {
                var i = dataToNormalize[dd];
                i -= min;
                i /= (-min + max);
                dataToNormalize[dd] = i - bias;
            }
        }

        public void GNormalize(
            float[] dataToNormalize
            )
        {
            if (dataToNormalize == null)
            {
                throw new ArgumentNullException("dataToNormalize");
            }

            var mean0 = dataToNormalize.Mean();

            var variance0 = dataToNormalize.GaussVariance();

            //var standardDeviation0 =
            //    (float)MathNet.Numerics.Statistics.Statistics.StandardDeviation(dataToNormalize.ToList().ConvertAll(j => (double)j));

            var sqrtVariance = (float)Math.Sqrt(variance0);

            //do remap
            for (var i = 0; i < dataToNormalize.Length; i++)
            {
                dataToNormalize[i] = (dataToNormalize[i] - mean0)/sqrtVariance;

                //dataToNormalize[i] -= mean0;
                //dataToNormalize[i] /= sqrtVariance;
            }
        }
    }


}
