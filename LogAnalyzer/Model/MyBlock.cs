using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalyzer.Model
{
    public class MyBlock
    {
        private Dictionary<string, DataLine> lines;
        private string mask;

        public Dictionary<string, DataLine> Lines
        {
            get
            {
                return lines;
            }
        }

        public MyBlock(Dictionary<string, DataLine> lines, string mask)
        {
            this.lines = lines;
        }
    }
}
