using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalyzer.Model.DataBlocks
{
    public class DataBlock2
    {
        private Dictionary<string, DataLine> lines;

        public DataBlock2(Dictionary<string, DataLine> lines)
        {
            this.lines = lines;
        }
    }
}
