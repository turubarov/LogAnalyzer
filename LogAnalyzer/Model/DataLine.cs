using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalyzer.Model
{
    public class DataLine
    {
        private Dictionary<string, Data> values;

        public Dictionary<string, Data> Values { get { return values; } }

        public DataLine(Dictionary<string, Data> values)
        {
            this.values = values;
        }

        public Data getValue(string valueType)
        {
            return values[valueType];
        }
    }
}
