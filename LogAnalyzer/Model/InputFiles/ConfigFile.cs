using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalyzer.Model
{
    class ConfigFile
    {
        private string[] pathToLogs;
        public string[] PathToLogs { get { return pathToLogs; } }

        private string[] block0Params;
        public string[] Block0Params { get { return block0Params; } }

        private string[] block2Params;
        public string[] Block2Params { get { return block2Params; } }

        public ConfigFile(string[] pathToLogs, string[] block0Params, string[] block2Params)
        {
            this.pathToLogs = pathToLogs;
            this.block0Params = block0Params;
            this.block2Params = block2Params;
        }
    }
}
