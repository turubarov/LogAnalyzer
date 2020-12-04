using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalyzer.Exceptions
{
    using System;

    public class ConfigParamException : Exception
    {
        public ConfigParamException()
        {
        }

        public ConfigParamException(string message)
            : base(message)
        {
        }

        public ConfigParamException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
