using System;
using System.Text.RegularExpressions;

namespace LogAnalyzer.Model
{
    enum DataType { Time, Number, String}
    public class MyData
    {
        private int value;
        private string stringValue;
        private DataType type;

        public MyData(string stringValue)
        {
            Regex timeRegex = new Regex(@"^(\d{2}):(\d{2}):(\d{2})$");
            Regex numbRegex = new Regex(@"^[0-9]+$");
            if (timeRegex.IsMatch(stringValue))
                type = DataType.Time;
            else if (numbRegex.IsMatch(stringValue))
                type = DataType.Number;
            else
                type = DataType.String;
            switch (type)
            {
                case DataType.Time:
                    value = 0;
                    Match m = timeRegex.Match(stringValue);
                    int d = 3600;
                    for (int i = 0; i < 3; i++)
                    {
                        string s = m.Groups[i + 1].ToString();
                        value += int.Parse(m.Groups[i + 1].ToString()) * d;
                        d /= 60;
                    }
                    break;
                case DataType.Number:
                    value = int.Parse(stringValue);
                    break;
                case DataType.String:
                    value = 0;
                    break;
            }
            this.stringValue = stringValue;
        }

        public string StringValue
        {
            get
            {
                return stringValue;
            }
        }

        public int Value
        {
            get
            {
                return value;
            }
        }

        public override string ToString()
        {
            return stringValue;
        }
    }
}
