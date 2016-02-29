using System;
using System.Diagnostics;
using Banana.MLP.Function;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Banana.MLP.Tests.Function.CSharp.Derivative
{
    internal class ActivationFunctionDerivativeTests
    {
        private const float DeltaX = 0.01f;

        public void ExecuteTests(
            IFunction f,
            float lleft = -100.5f,
            float rright = 100f,
            float step = 0.17f,
            float allowedDerivativeEpsilon = 0.001f
            )
        {
            if (f == null)
            {
                throw new ArgumentNullException("f");
            }

            Debug.Write("Разница: ");

            for (var cc = lleft; cc < rright; cc += step)
            {
                var left = cc - DeltaX;
                var right = cc + DeltaX;

                var leftValue = f.Compute(left);
                var rightValue = f.Compute(right);

                var cDerivative = (rightValue - leftValue) / (2f * DeltaX);
                var fDerivative = f.ComputeFirstDerivative(cc);

                var diff = Math.Abs(cDerivative - fDerivative);

                Debug.Write(diff + "   ");

                Assert.IsTrue(diff <= allowedDerivativeEpsilon);
            }
        }
    }
}