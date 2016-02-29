using System.Text;
using System.Threading.Tasks;

namespace Banana.Data.Set
{
    public interface IDataSet
    {
        int Count
        {
            get;
        }

        bool IsAutoencoderDataSet
        {
            get;
        }

        int InputLength
        {
            get;
        }

        int OutputLength
        {
            get;
        }

        IDataSetIterator StartIterate(
            );
    }
}
