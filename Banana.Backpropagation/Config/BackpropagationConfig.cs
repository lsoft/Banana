using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banana.Backpropagation.Config
{
    public interface IBackpropagationConfig
    {
        int ItemCountCacheSize
        {
            get;
        }
    }

    public class BackpropagationConfig : IBackpropagationConfig
    {
        public int ItemCountCacheSize
        {
            get;
            private set;
        }

        public BackpropagationConfig(
            int itemCountCacheSize
            )
        {
            if (itemCountCacheSize <= 0)
            {
                throw new ArgumentException("itemCountCacheSize <= 0");
            }

            ItemCountCacheSize = itemCountCacheSize;
        }
    }
}
