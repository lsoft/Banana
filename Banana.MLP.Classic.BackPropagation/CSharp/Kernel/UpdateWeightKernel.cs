using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banana.Common;
using Banana.MLP.Container.CSharp;

namespace Banana.MLP.Classic.BackPropagation.CSharp.Kernel
{
    internal class UpdateWeightKernel
    {
        public void UpdateWeigths(
            ICSharpLayerContainer layerContainer,
            int batchSize
            )
        {
            if (layerContainer == null)
            {
                throw new ArgumentNullException("layerContainer");
            }

            var fBatchSize = (float) batchSize;

            ForHelper.ForBetween(0, layerContainer.WeightMem.Length, cc =>
            {
                layerContainer.WeightMem[cc] += layerContainer.NablaWeights[cc] / fBatchSize;
            }
            ); //ForHelper.ForBetween

            ForHelper.ForBetween(0, layerContainer.BiasMem.Length, cc =>
            {
                layerContainer.BiasMem[cc] += layerContainer.NablaBiases[cc] / fBatchSize;
            }
            ); //ForHelper.ForBetween

        }
    }
}
