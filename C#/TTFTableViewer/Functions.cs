using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTFTableViewer
{
    public class Functions
    {
        public static unsafe uint CalcTableChecksum(uint* table, int length)
        {
            uint sum = 0;
            uint* endPtr = table + ((length + 3) & ~3) / sizeof(uint);

            while (table < endPtr)
                sum += *table++;

            return sum;
            
        }
    }
}
