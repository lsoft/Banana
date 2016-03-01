using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCL.Net.Wrapper;
using OpenCL.Net.Wrapper.Mem.Data;

namespace Banana.Common.Sort
{
    public class AMDBitonicSorter
    {
        private readonly Kernel _kernel;

        public AMDBitonicSorter(
            CLProvider clProvider)
        {
            if (clProvider == null)
            {
                throw new ArgumentNullException("clProvider");
            }

            _kernel = clProvider.CreateKernel(
               OpenCL.Net.Wrapper.Properties.Resources.BitonicSort,
                "BitonicSortKernel"
                );
        }

        public void Sort(
            MemByte dataMem,
            ulong totalElementCountPlusOverhead
            )
        {
            if (dataMem == null)
            {
                throw new ArgumentNullException("dataMem");
            }

            if (totalElementCountPlusOverhead < 4)
            {
                throw new ArgumentOutOfRangeException("totalElementCountPlusOverhead");
            }

            var log2d = Math.Log(totalElementCountPlusOverhead, 2);
            if ((log2d % 1) > double.Epsilon)
            {
                throw new ArgumentException(
                    "Element amount should be equal of any power of 2.",
                    "totalElementCountPlusOverhead"
                    );
            }

            var localSizes = new uint[]
            {
                2,
                4,
                8,
                16,
                32,
                64,
                128
            };

            var localSize = localSizes
                .Where(j => (2 * j) <= totalElementCountPlusOverhead)
                .Max();

            //выполняем алгоритм

            uint numStages = 0;
            for (var temp = totalElementCountPlusOverhead; temp > 1; temp >>= 1)
            {
                ++numStages;
            }

            var globalThreads = new ulong[] { totalElementCountPlusOverhead / 2 };
            var localThreads = new ulong[] { localSize };

            for (var stage = 0; stage < numStages; ++stage)
            {
                // Every stage has stage + 1 passes
                for (var passOfStage = 0; passOfStage < stage + 1; ++passOfStage)
                {
                    //*
                    //* Enqueue a kernel run call.
                    //* For simplicity, the groupsize used is 1.
                    //*
                    //* Each thread writes a sorted pair.
                    //* So, the number of  threads (global) is half the length.
                    //*
                    _kernel
                        .SetKernelArgMem(0, dataMem)
                        .SetKernelArg(1, sizeof(uint), stage)
                        .SetKernelArg(2, sizeof(uint), passOfStage)
                        .SetKernelArg(3, sizeof(uint), (uint)1)
                        .EnqueueNDRangeKernel(
                            globalThreads,
                            localThreads
                            );
                }
            }
        }

    }
}
