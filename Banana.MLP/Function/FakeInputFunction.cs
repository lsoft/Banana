using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banana.Exception;
using OpenCL.Net.Wrapper;

namespace Banana.MLP.Function
{
    public class FakeInputFunction : IFunction
    {
        public static readonly IFunction FakeSingleton = new FakeInputFunction();

        public string ShortName
        {
            get
            {
                return
                    "Input";
            }
        }

        public float Compute(float x)
        {
            throw new BananaException(
                BananaErrorEnum.NotApplicable
                );
        }

        public float ComputeFirstDerivative(float x)
        {
            throw new BananaException(
                BananaErrorEnum.NotApplicable
                );
        }

        public string GetOpenCLActivationMethod(string methodName, VectorizationSizeEnum vse)
        {
            throw new BananaException(
                BananaErrorEnum.NotApplicable
                );
        }

        public string GetOpenCLDerivativeMethod(string methodName, VectorizationSizeEnum vse)
        {
            throw new BananaException(
                BananaErrorEnum.NotApplicable
                );
        }
    }
}
