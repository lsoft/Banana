using System;
using System.Diagnostics;
using Banana.Common.Others;
using Banana.MLP.Function;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCL.Net;
using OpenCL.Net.Wrapper;
using OpenCL.Net.Wrapper.Mem;

namespace Banana.MLP.Tests.Function.OpenCL.Value
{
    internal class ActivationFunctionValueTests
    {
        public void ExecuteTests(
            IFunction f,
            float left = -100.5f,
            float right = 100f,
            float step = 0.17f,
            float allowedDerivativeEpsilon = 0.00001f
            )
        {
            if (f == null)
            {
                throw new ArgumentNullException("f");
            }

            using (var clProvider = new CLProvider())
            {
                var diffk = CalculateKernel.Replace(
                    "<GetActivationFunction>",
                    f.GetOpenCLActivationMethod(
                        "GetActivationFunction",
                        VectorizationSizeEnum.NoVectorization));

                var calculateKernel = clProvider.CreateKernel(
                    diffk,
                    "CalculateValuesKernel");

                var count = (int)((right - left) / step);

                var ccMem = clProvider.CreateFloatMem(
                    count,
                    MemFlags.CopyHostPtr | MemFlags.ReadWrite
                    );

                //считаем через С#
                var values = new float[count];
                for (var cc = 0; cc < count; cc++)
                {
                    var currentcc = left + step * cc;
                    ccMem.Array[cc] = currentcc;

                    var value = f.Compute(currentcc);
                    values[cc] = value;
                }

                var valueMem = clProvider.CreateFloatMem(
                    count,
                    MemFlags.CopyHostPtr | MemFlags.ReadWrite
                    );

                ccMem.Write(BlockModeEnum.Blocking);
                valueMem.Write(BlockModeEnum.Blocking);

                calculateKernel
                    .SetKernelArgMem(0, ccMem)
                    .SetKernelArgMem(1, valueMem)
                    .EnqueueNDRangeKernel(count)
                    ;

                clProvider.QueueFinish();

                valueMem.Read(BlockModeEnum.Blocking);

                var diffArray = ArrayHelper.DiffArrays(
                    values,
                    valueMem.Array
                    );

                Debug.WriteLine(
                    string.Format(
                        "Разница: {0}",
                        string.Join(
                            "   ",
                            diffArray)));

                for (var cc = 0; cc < diffArray.Length; cc++)
                {
                    var diff = Math.Abs(diffArray[cc]);

                    Assert.IsTrue(diff <= allowedDerivativeEpsilon);
                }

            }
        }


        private const string CalculateKernel = @"
<GetActivationFunction>

__kernel void CalculateValuesKernel(
    __global float * ccMem,
    __global float * valueMem
    )
{
    int kernelIndex = get_global_id(0);

    float currentcc = ccMem[kernelIndex];

    float value = GetActivationFunction(currentcc);

    valueMem[kernelIndex] = value;
}

";
    }
}