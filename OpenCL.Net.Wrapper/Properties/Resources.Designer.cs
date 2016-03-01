﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OpenCL.Net.Wrapper.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("OpenCL.Net.Wrapper.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to typedef struct
        ///{
        ///    uint AIndex;
        ///    uint BIndex;
        ///    float Distance;
        ///} SortItem;
        ///
        ///__inline ulong GetKey(SortItem d)
        ///{
        ///    ulong a = d.AIndex;
        ///
        ///    return
        ///        (a &lt;&lt; 32) + d.BIndex;
        ///}
        ///
        ///__kernel void BitonicSortKernel(
        ///    __global SortItem * theArray,
        ///    const uint stage, 
        ///    const uint passOfStage,
        ///    const uint direction
        ///    )
        ///{
        ///    uint sortIncreasing = direction;
        ///    ulong threadId = get_global_id(0);
        ///     
        ///    ulong pairDistance = 1 &lt;&lt; (stage - passOfStage);
        ///    ulong b [rest of string was truncated]&quot;;.
        /// </summary>
        public static string BitonicSort {
            get {
                return ResourceManager.GetString("BitonicSort", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to typedef struct
        ///{
        ///    float Sum;
        ///    float C;
        ///} KahanAccumulator;
        ///
        ///inline KahanAccumulator GetKahanAcc(
        ///    float startValue
        ///    )
        ///{
        ///    KahanAccumulator result;
        ///    result.Sum = startValue;
        ///    result.C = 0.0;
        ///
        ///    return result;
        ///}
        ///
        ///inline KahanAccumulator GetEmptyKahanAcc(
        ///    )
        ///{
        ///    KahanAccumulator result;
        ///    result.Sum = 0.0;
        ///    result.C = 0.0;
        ///
        ///    return result;
        ///}
        ///
        ///inline void KahanAddElement(
        ///    KahanAccumulator* acc,
        ///    float dataItem
        ///    )
        ///{
        ///    float y = dataIt [rest of string was truncated]&quot;;.
        /// </summary>
        public static string KahanAlgorithm {
            get {
                return ResourceManager.GetString("KahanAlgorithm", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to typedef struct
        ///{
        ///    float16 Sum;
        ///    float16 C;
        ///} KahanAccumulator16;
        ///
        ///inline float ReduceAcc16(
        ///    KahanAccumulator16* acc16
        ///    )
        ///{
        ///    KahanAccumulator acc = GetEmptyKahanAcc();
        ///
        ///    KahanAddElement(&amp;acc, acc16-&gt;Sum.s0);
        ///    KahanAddElement(&amp;acc, acc16-&gt;Sum.s1);
        ///    KahanAddElement(&amp;acc, acc16-&gt;Sum.s2);
        ///    KahanAddElement(&amp;acc, acc16-&gt;Sum.s3);
        ///    
        ///    KahanAddElement(&amp;acc, acc16-&gt;Sum.s4);
        ///    KahanAddElement(&amp;acc, acc16-&gt;Sum.s5);
        ///    KahanAddElement(&amp;acc, acc16-&gt;Sum.s6);
        ///    Kahan [rest of string was truncated]&quot;;.
        /// </summary>
        public static string KahanAlgorithm16 {
            get {
                return ResourceManager.GetString("KahanAlgorithm16", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to typedef struct
        ///{
        ///    float4 Sum;
        ///    float4 C;
        ///} KahanAccumulator4;
        ///
        ///inline float ReduceAcc4(
        ///    KahanAccumulator4* acc4
        ///    )
        ///{
        ///    KahanAccumulator acc = GetEmptyKahanAcc();
        ///
        ///    KahanAddElement(&amp;acc, acc4-&gt;Sum.s0);
        ///    KahanAddElement(&amp;acc, acc4-&gt;Sum.s1);
        ///    KahanAddElement(&amp;acc, acc4-&gt;Sum.s2);
        ///    KahanAddElement(&amp;acc, acc4-&gt;Sum.s3);
        ///
        ///    return 
        ///        acc.Sum;
        ///}
        ///
        ///inline KahanAccumulator4 GetEmptyKahanAcc4(
        ///    )
        ///{
        ///    KahanAccumulator4 result;
        ///    result.Sum = 0.0;
        ///    res [rest of string was truncated]&quot;;.
        /// </summary>
        public static string KahanAlgorithm4 {
            get {
                return ResourceManager.GetString("KahanAlgorithm4", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /*
        /////naive implementation (for testing only)
        /////local size must be equal partialDotProduct size
        ///inline void WarpReductionToFirstElement(
        ///    __local float *partialDotProduct)
        ///{
        ///    if(get_local_id(0) == 0)
        ///    {
        ///        float sum = 0;
        ///        for(int c = 0; c &lt; get_local_size(0); c++)
        ///        {
        ///            sum += partialDotProduct[c];
        ///        }
        ///        partialDotProduct[0] = sum;
        ///    }
        ///    barrier(CLK_LOCAL_MEM_FENCE);
        ///}
        /////*/
        ///
        ///
        /////reduction with 2x rate
        /////local size must be equal partialD [rest of string was truncated]&quot;;.
        /// </summary>
        public static string Reduction {
            get {
                return ResourceManager.GetString("Reduction", resourceCulture);
            }
        }
    }
}
