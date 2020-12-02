using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalyzer.Model
{
    public class MyLine
    {
        private Dictionary<string, MyData> value;

        public Dictionary<string, MyData>  Value
        {
            get
            {
                return value;
            }
        }

        public MyLine(Dictionary<string, MyData> value)
        {
            this.value = value;
            //value = new Dictionary<string, MyData>();
        }
    }
}
