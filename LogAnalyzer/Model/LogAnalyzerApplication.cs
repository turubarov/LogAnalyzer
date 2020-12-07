using LogAnalyzer.Model.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalyzer.Model
{
    public class LogAnalyzerApplication
    {
        private List<LogFile> logFiles;
        public List<LogFile> LogFiles { get { return logFiles; } }

        private ConfigFile configFile;
        public ConfigFile ConfigFile { get { return configFile; } }

        public LogAnalyzerApplication()
        {
            ConfigFile configFile = ConfigParser.Parse();
            if (configFile == null)
            {
                System.Windows.Application.Current.Shutdown();
                return;
            }
            logFiles = LogsParser.Parse(configFile);
            this.configFile = configFile;
        }
    }
}
