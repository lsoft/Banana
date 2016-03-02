using System;
using System.Diagnostics;
using System.Linq;
using Banana.Common.Others;
using Banana.MLP.Classic.ForwardPropagation.Layer;
using Banana.MLP.Configuration.Layer;
using Banana.MLP.Container.Layer.CSharp;
using Banana.MLP.Dim;
using Banana.MLP.Function;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Banana.MLP.Tests.LayerPropagator.CSharp
{
    [TestClass]
    public class CSharpLayerPropagatorFixture
    {
        private const float Epsilon = 1e-6f;

        [TestMethod]
        public void Test_1_1()
        {
            const int previousLayerNeuronCount = 1;
            const int currentLayerNeuronCount = 1;

            FullConnectedLayerConfiguration l;
            CSharpLayerContainer plc;
            CSharpLayerContainer clc;
            CSharpLayerPropagator lp;
            ConstuctComponents(
                previousLayerNeuronCount,
                currentLayerNeuronCount,
                out l,
                out plc,
                out clc,
                out lp);

            clc.WeightMem[0] = 0.5f;
            plc.NetMem[0] = 1f;
            plc.StateMem[0] = 1f;

            lp.ComputeLayer();
            lp.WaitForCalculationFinished();

            const float CorrectValue = 0.5f;

            Debug.WriteLine(
                "Received: {0}. Expected: {1}.",
                clc.StateMem[0],
                CorrectValue
                );
            Assert.IsTrue(clc.StateMem[0].IsEquals(CorrectValue, Epsilon));

        }

        [TestMethod]
        public void Test_2_1()
        {

            const int previousLayerNeuronCount = 2;
            const int currentLayerNeuronCount = 1;

            FullConnectedLayerConfiguration l;
            CSharpLayerContainer plc;
            CSharpLayerContainer clc;
            CSharpLayerPropagator lp;
            ConstuctComponents(
                previousLayerNeuronCount,
                currentLayerNeuronCount,
                out l,
                out plc,
                out clc,
                out lp);

            clc.WeightMem[0] = 0.5f;
            clc.WeightMem[1] = -0.5f;

            plc.NetMem[0] = -2f;
            plc.StateMem[0] = -2f;
            plc.NetMem[1] = 1f;
            plc.StateMem[1] = 1f;

            lp.ComputeLayer();
            lp.WaitForCalculationFinished();

            const float CorrectValue = -1.5f;

            Assert.IsTrue(clc.StateMem[0].IsEquals(CorrectValue, Epsilon));

        }

        [TestMethod]
        public void Test0_40_1()
        {
            const int previousLayerNeuronCount = 40;
            const int currentLayerNeuronCount = 1;

            FullConnectedLayerConfiguration l;
            CSharpLayerContainer plc;
            CSharpLayerContainer clc;
            CSharpLayerPropagator lp;
            ConstuctComponents(
                previousLayerNeuronCount,
                currentLayerNeuronCount,
                out l,
                out plc,
                out clc,
                out lp);

            clc.WeightMem.Fill((a) => (float)a);

            plc.NetMem.Fill(1f);
            plc.StateMem.Fill(1f);

            lp.ComputeLayer();
            lp.WaitForCalculationFinished();

            float correctValue = Enumerable.Range(0, previousLayerNeuronCount).Sum();

            Assert.IsTrue(clc.StateMem[0].IsEquals(correctValue, Epsilon));

        }

        [TestMethod]
        public void Test1_40_1()
        {
            const int previousLayerNeuronCount = 40;
            const int currentLayerNeuronCount = 1;

            FullConnectedLayerConfiguration l;
            CSharpLayerContainer plc;
            CSharpLayerContainer clc;
            CSharpLayerPropagator lp;
            ConstuctComponents(
                previousLayerNeuronCount,
                currentLayerNeuronCount,
                out l,
                out plc,
                out clc,
                out lp);

            clc.WeightMem.Fill((a) => (float)a);

            plc.NetMem.Fill((a) => (float)a);
            plc.StateMem.Fill((a) => (float)a);

            lp.ComputeLayer();
            lp.WaitForCalculationFinished();

            var correctArray = Enumerable.Range(0, previousLayerNeuronCount).ToArray();
            correctArray.TransformInPlace((a) => a * a);
            float correctValue = correctArray.Sum();

            Assert.IsTrue(clc.StateMem[0].IsEquals(correctValue, Epsilon));

        }

        private static void ConstuctComponents(
            int previousLayerNeuronCount,
            int currentLayerNeuronCount,
            out FullConnectedLayerConfiguration l,
            out CSharpLayerContainer plc,
            out CSharpLayerContainer clc,
            out CSharpLayerPropagator lp)
        {
            l = new FullConnectedLayerConfiguration(
                new Dimension(1, currentLayerNeuronCount),
                new LinearFunction(1f)
                );

            plc = new CSharpLayerContainer(
                new InputLayerConfiguration(
                    previousLayerNeuronCount
                    )
                );

            clc = new CSharpLayerContainer(
                plc.Configuration,
                l
                );

            lp = new CSharpLayerPropagator(
                plc,
                clc
                );
        }

    }
}
