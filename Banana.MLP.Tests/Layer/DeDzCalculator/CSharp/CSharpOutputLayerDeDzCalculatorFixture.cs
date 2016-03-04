using System;
using Banana.MLP.Classic.BackPropagation.DeDyAggregator;
using Banana.MLP.Classic.BackPropagation.DeDzCalculator.Output;
using Banana.MLP.Configuration.Layer;
using Banana.MLP.Container.Layer.CSharp;
using Banana.MLP.Dim;
using Banana.MLP.Function;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Banana.MLP.Tests.Layer.DeDzCalculator.CSharp
{
    [TestClass]
    public class CSharpOutputLayerDeDzCalculatorFixture
    {
        [TestMethod]
        public void Test0()
        {
            int previousLayerNeuronCount = 100;
            int currentLayerNeuronCount = 200;

            var plconf = new InputLayerConfiguration(
                previousLayerNeuronCount
                );

            var clconf = new FullConnectedLayerConfiguration(
                new Dimension(1, currentLayerNeuronCount),
                new LinearFunction(1f)
                );

            var clc = new CSharpLayerContainer(
                plconf,
                clconf
                );


            var deDzCalculator = new CSharpOutputLayerDeDzCalculator(
                plconf,
                clc
                );

            var rnd = new Random();

            ////fill random weghts
            //for (var ni = 0; ni < currentLayerNeuronCount; ni++)
            //{
            //    for (var wi = 0; wi < previousLayerNeuronCount; wi++)
            //    {
            //        var wwi = ComputeWeightIndex(previousLayerNeuronCount, ni) + wi;

            //        clc.WeightMem[wwi] = (float) rnd.NextDouble()*2 - 1;
            //    }
            //}

            ////fill random dEdZ
            //for (var ni = 0; ni < currentLayerNeuronCount; ni++)
            //{
            //    clc.DeDz[ni] = (float)rnd.NextDouble() * 2 - 1;
            //}

            //deDyAggregator.Aggregate();
        }

        private static int ComputeWeightIndex(
            int previousLayerNeuronCount,
            int neuronIndex
            )
        {
            return
                previousLayerNeuronCount * neuronIndex;
        }

    }
}
