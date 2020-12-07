using LogAnalyzer.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace LogAnalyzer.Model.Parsers
{
    public class ConfigParser
    {
        private static string[] REQUIRED_CONFIG_PARAMS = new string[] { "Directory", "Block0", "Block1", "Block2" };

        public static ConfigFile Parse()
        {
            try
            {
                Dictionary<string, string> confStrings = readConfigFile();
                string[] pathToLogs = getPathsToLogs(confStrings["Directory"]);
                string[] block0Params = getBlock0Params(confStrings["Block0"]);
                string[] block2Params = getBlock0Params(confStrings["Block2"]);
                ConfigFile configFile = new ConfigFile(pathToLogs, block0Params, block2Params);
                return configFile;
            }
            catch (ConfigParamException e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка при чтении файла \n" + e.Message);
                return null;
            }
        }

        private static Dictionary<string, string> readConfigFile()
        {
            string[] confText = null;
            Dictionary<string, string> confStrings;

            confText = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\config.txt");
            try
            { 
                confStrings = confText.ToDictionary(s => s.Split(new char[] { '=' }, 2)[0], s => s.Split(new char[] { '=' }, 2)[1]);
            }
            catch (IndexOutOfRangeException)
            {
                throw new ConfigParamException("Неправильный формат параметра в конфигурационном файле");
            }
            foreach (string param in REQUIRED_CONFIG_PARAMS)
            {
                if (!confStrings.ContainsKey(param))
                {
                    throw new ConfigParamException("Не найден параметр " + param + " в конфигурационном файле");
                }
            }
            return confStrings;
        }

        private static string[] getPathsToLogs(string logDirectory)
        {
            return Directory.GetFiles(Directory.GetCurrentDirectory() + logDirectory);
        }

        private static string[] getBlock0Params(string block0String)
        {
            Regex regex = new Regex(@"%[\w\s]+%");

            MatchCollection matches = regex.Matches(block0String);
            string[] block0Params = new string[matches.Count];
            for (int i = 0; i < matches.Count; i++)
            {
                block0Params[i] = matches[i].Value.Substring(1, matches[i].Value.Length - 2);
            }
            return block0Params;
        }

        private static string[] getBlock2Params(string block2String)
        {
            Regex regex = new Regex(@"%[\w\s]+%");

            MatchCollection matches = regex.Matches(block2String);
            string[] block2Params = new string[matches.Count];
            for (int i = 0; i < matches.Count; i++)
            {
                block2Params[i] = matches[i].Value.Substring(1, matches[i].Value.Length - 2);
            }
            return block2Params;
        }
    }
}
