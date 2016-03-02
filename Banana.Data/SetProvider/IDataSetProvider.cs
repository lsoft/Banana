using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banana.Data.Set;

namespace Banana.Data.SetProvider
{
    public interface IDataSetProvider
    {
        int Count
        {
            get;
        }

        IDataSet GetDataSet(int epochNumber);

    }
}
