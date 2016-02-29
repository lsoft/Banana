using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCL.Net.Wrapper.Helper
{
    public class MemModifierHelper
    {
        public static string GetModifierSuffix(MemModifierEnum mme)
        {
            switch (mme)
            {
                case MemModifierEnum.NotSpecified:
                    return string.Empty;
                case MemModifierEnum.Local:
                    return "__local";
                case MemModifierEnum.Global:
                    return "__global";
                default:
                    throw new ArgumentOutOfRangeException("mme");
            }
        }
    }
}
