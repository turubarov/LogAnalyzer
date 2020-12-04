using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalyzer.Model
{
    public class DataLine
    {
        private Dictionary<string, Data> value;

        public Dictionary<string, Data>  Value
        {
            get
            {
                return value;
            }
        }

        public DataLine(Dictionary<string, Data> value)
        {
            this.value = value;
            //value = new Dictionary<string, MyData>();
        }
    }
}
