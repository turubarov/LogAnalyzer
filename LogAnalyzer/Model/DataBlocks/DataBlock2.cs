using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalyzer.Model.DataBlocks
{
    public class DataBlock2
    {
        private Dictionary<string, DataLine> dataLines;

        public DataBlock2(Dictionary<string, DataLine> dataLines)
        {
            this.dataLines = dataLines;
        }

        public DataLine getDataLine(string tabType)
        {
            return dataLines[tabType];
        }

        public string[] getTabTypes()
        {
            return dataLines.Keys.ToArray();
        }
    }
}
