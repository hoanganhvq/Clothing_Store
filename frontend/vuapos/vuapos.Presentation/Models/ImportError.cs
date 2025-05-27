using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vuapos.Presentation.Models
{
    public class ImportError
    {
        public int Row { get; set; }
        public string Reason { get; set; }

        public ImportError(int row, string reason)
        {
            Row = row;
            Reason = reason;
        }
    }
}
