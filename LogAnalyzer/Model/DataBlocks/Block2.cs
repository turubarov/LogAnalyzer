using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalyzer.Model.DataBlocks
{
    class Block2
    {
        private Dictionary<string, DataLine> lines;

        public Block2(Dictionary<string, DataLine> lines)
        {
            this.lines = lines;
        }
    }
}
