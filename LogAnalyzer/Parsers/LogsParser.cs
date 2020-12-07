using LogAnalyzer.Model.DataBlocks;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace LogAnalyzer.Model.Parsers
{
    public class LogsParser
    {
        public static List<LogFile> Parse(ConfigFile configFile)
        {
            List<LogFile> logFiles = new List<LogFile>();
            foreach (string pathToLog in configFile.PathToLogs)
            {
                string line;
                using (StreamReader sr = new StreamReader(pathToLog, Encoding.Default))
                {
                    skipLine(sr);

                    // чтение нулевого блока данных
                    line = skipEmptyLines(sr);
                    Dictionary<string, DataLine> dataLines = new Dictionary<string, DataLine>();
                    KeyValuePair<string, DataLine> dataLine;

                    do
                    {
                        dataLine = parseBlock0String(line, configFile.Block0Params);
                        dataLines.Add(dataLine.Key, dataLine.Value);
                    }
                    while ((line = sr.ReadLine().Trim()) != "");
                    DataBlock0 dataBlock0 = new DataBlock0(dataLines);
                    skipEmptyLines(sr);
                    skipLine(sr);
                    line = skipEmptyLines(sr);

                    // чтение первого блока данных
                    string[] tabTypes = dataBlock0.getTabTypes();
                    int countOfTabTypes = tabTypes.Length;
                    int[,] countOfTransitions = new int[countOfTabTypes, countOfTabTypes];
                    do
                    {
                        parseBlock1String(line, tabTypes, ref countOfTransitions);
                    }
                    while ((line = sr.ReadLine().Trim()) != "");
                    DataBlock1 dataBlock1 = new DataBlock1(countOfTransitions, tabTypes);
                    skipEmptyLines(sr);
                    skipLine(sr);
                    line = skipEmptyLines(sr);

                    // чтение второго блока данных
                    dataLines = new Dictionary<string, DataLine>() ;
                    do
                    {
                        dataLine = parseBlock2String(line, configFile.Block0Params);
                        dataLines.Add(dataLine.Key, dataLine.Value);
                    }
                    while ((line = sr.ReadLine().Trim()) != "");
                    DataBlock2 dataBlock2 = new DataBlock2(dataLines);
                    LogFile logFile = new LogFile(Path.GetFileName(pathToLog), dataBlock0, dataBlock1, dataBlock2);
                    logFiles.Add(logFile);
                }
            }
            return logFiles;
        }

        private static KeyValuePair<string, DataLine> parseBlock0String(string input, string[] block0Params)
        {
            Dictionary<string, Data> values = new Dictionary<string, Data>();
            Regex regex = new Regex(@"(\d{2}:\d{2}:\d{2})|(\w+)");
            MatchCollection matches = regex.Matches(input);
            for (int i = 1; i < matches.Count; i++)
            {
                values[block0Params[i]] = new Data(matches[i].Value);
            }
            DataLine dataLine = new DataLine(values);
            return new KeyValuePair<string, DataLine>(matches[0].Value, dataLine);
        }

        private static void parseBlock1String(string input, string[] tabTypes, ref int[,] countOfTransitions)
        {
            Dictionary<string, Data> values = new Dictionary<string, Data>();
            Regex regex = new Regex(@"(\w+)");
            MatchCollection matches = regex.Matches(input);
            string tabFirst = matches[0].Value;
            string tabSecond = matches[1].Value;
            int countOfTransition = int.Parse(matches[2].Value);
            int tabFirstIndex = 0;
            int tabSecondIndex = 0;
            for (int i = 0; i < tabTypes.Length; i++)
            {
                if (tabTypes[i] == tabFirst)
                    tabFirstIndex = i;
                if (tabTypes[i] == tabSecond)
                    tabSecondIndex = i;
                if (tabFirstIndex != 0 && tabSecondIndex != 0)
                    break;
            }
            countOfTransitions[tabFirstIndex, tabSecondIndex] = countOfTransition;
        }

        private static KeyValuePair<string, DataLine> parseBlock2String(string input, string[] block2Params)
        {
            Dictionary<string, Data> values = new Dictionary<string, Data>();
            Regex regex = new Regex(@"(\d{2}:\d{2}:\d{2})|(\w+)");
            MatchCollection matches = regex.Matches(input);
            for (int i = 2; i < matches.Count; i++)
            {
                values[block2Params[i]] = new Data(matches[i].Value);
            }
            if (matches.Count == 4)
            {
                values[block2Params[4]] = new Data("0"); 
            }
            DataLine dataLine = new DataLine(values);
            return new KeyValuePair<string, DataLine>(matches[0].Value + matches[1].Value, dataLine);
        }

        private static void skipLine(StreamReader sr)
        {
            sr.ReadLine();
        }

        private static string skipEmptyLines(StreamReader sr)
        {
            string line;
            while ((line = sr.ReadLine().Trim()) == "")
            {
                string ddd = line;
            }
            return line;
        }
    }
}
