using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalyzer.Model.DataBlocks
{
    public class DataBlock0
    {
        private Dictionary<string, DataLine> dataLines;

        public DataBlock0(Dictionary<string, DataLine> dataLines)
        {
            this.dataLines = dataLines;
        }

        public string[] getTabTypes()
        {
            return dataLines.Keys.ToArray();
        }
    }
}
