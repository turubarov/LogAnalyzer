using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalyzer.Model
{
    public class MyBlock
    {
        private Dictionary<string, MyLine> lines;
        private string mask;

        public Dictionary<string, MyLine> Lines
        {
            get
            {
                return lines;
            }
        }

        public MyBlock(Dictionary<string, MyLine> lines, string mask)
        {
            this.lines = lines;
        }
    }
}
