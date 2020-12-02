using LogAnalyzer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LogAnalyzer.Ulils
{
    public class MyFileReader
    {
        private string[] masks;
        private string[,] maskParams;
        private string[] lineParams;

        private const int COUNT_BLOCKS = 3;
        private const int MAX_PARAMS = 20;

        public List<LogFile> Read()
        {
            Dictionary<string, string> configParams = getDictionaryFromConfig();
            string[] logNames = getLogFiles(configParams);
            masks = getLogMasks(configParams);
            maskParams = getMaskParams(masks);

            List<LogFile> logFiles = new List<LogFile>();
            foreach (string log in logNames) 
            {
                List<MyBlock> blocks = readLogFile(log);
                logFiles.Add(new LogFile(Path.GetFileName(log), blocks, maskParams, lineParams));
            }
            return logFiles;
        }

        private List<MyBlock> readLogFile (String logName)
        {
            List<MyBlock> blocks = new List<MyBlock>();
            using (StreamReader sr = new StreamReader(logName, Encoding.Default))
            {
                string line;
                List<string> lParams = new List<string>();
                while ((line = sr.ReadLine().Trim()) != "") ;
                for (int i = 0; i < COUNT_BLOCKS; i++)
                {
                    Dictionary<string, MyLine> lines = new Dictionary<string, MyLine>();
                    while ((line = sr.ReadLine().Trim()) != "")
                    {
                        Dictionary<string, MyData> tmp = parseLogString(line, i);
                        string nameLine = tmp[maskParams[i, 0]].StringValue;
                        
                        tmp.Remove(maskParams[i, 0]);
                        MyLine l = new MyLine(tmp);
                        lines[nameLine] = l;
                        if (i == 0)
                            lParams.Add(nameLine);
                    }
                    blocks.Add(new MyBlock(lines, ""));
                    sr.ReadLine();
                    sr.ReadLine();
                    sr.ReadLine();
                }
                lineParams = lParams.ToArray();
            }
            return blocks;
        }

        private Dictionary<string, MyData> parseLogString(string input, int blockNumber)
        {
            Dictionary<string, MyData> result = new Dictionary<string, MyData>();
            Regex regex = new Regex(@"(\d{2}:\d{2}:\d{2})|(\w+)");
            MatchCollection matches = regex.Matches(input);
            for (int i = 0; i< matches.Count; i++)
            {
                result[maskParams[blockNumber, i]] = new MyData(matches[i].Value);
            }
            return result;
        }

        private Dictionary<string, string> getDictionaryFromConfig()
        {
            string curDir = Directory.GetCurrentDirectory();
            string[] test = curDir.Split(new char[] { '='}, 2);
            string[] configText = File.ReadAllLines(curDir + "\\config.txt");
            return configText.ToDictionary(s => s.Split(new char[] { '=' }, 2)[0], s => s.Split(new char[] { '=' }, 2)[1]);
        }

        private string[] getLogFiles(Dictionary<string, string> conf)
        {
            string pathToLog = conf["Directory"];
            string[] logs = Directory.GetFiles(Directory.GetCurrentDirectory() + pathToLog);
            return logs;
        }

        private string[,] getMaskParams(string[] masks)
        {
            string[,] maskParams = new string[COUNT_BLOCKS, MAX_PARAMS];
            Regex regex = new Regex(@"%[\w]+%");
            for (int i = 0; i < COUNT_BLOCKS; i++)
            {
                MatchCollection matches = regex.Matches(masks[i]);
                for (int j = 0; j < matches.Count; j++)
                {
                    maskParams[i,j] = matches[j].Value.Substring(1, matches[j].Value.Length - 2);
                }
            }
            
            return maskParams;
        }

        private string[] getLogMasks(Dictionary<string, string> conf)
        {
            string[] masks = new string[COUNT_BLOCKS];
            for (int i = 0; i < COUNT_BLOCKS; i++)
            {

            }
            masks[0] = conf["Block0"];
            masks[1] = conf["Block1"];
            masks[2] = conf["Block2"];
            return masks;
        }
    }
}
