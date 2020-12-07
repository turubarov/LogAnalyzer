using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using LogAnalyzer.Model;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using LiveCharts.Defaults;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using LogAnalyzer.Model.Parsers;
using LogAnalyzer.ViewModel;

namespace LogAnalyzer
{
    public class LogAnalyzerApplicationVM : INotifyPropertyChanged
    {

        [DllImport("shlwapi.dll")]
        public static extern int ColorHLSToRGB(int H, int L, int S);

        private List<LogFileVM> logFilesVM;
        public List<LogFileVM> LogFilesVM { get { return logFilesVM; } }

        private ConfigFile configFile;

        private string selectedTabType;

        private LogFileVM selectedLog;
        public LogFileVM SelectedLog
        {
            get
            {
                return selectedLog;
            }
            set
            {
                selectedTabType = selectedLog.SelectedTabType;
                selectedLog = value;
                selectedLog.SelectedTabType = selectedTabType;
                OnPropertyChanged("SelectedLog");
            }
        }

        private SeriesCollection block0SeriesUp;
        public SeriesCollection Block0SeriesUp { get { return block0SeriesUp; } }

        private SeriesCollection block0SeriesBottom;
        public SeriesCollection Block0SeriesBottom { get { return block0SeriesBottom; } }

        private LogAnalyzerApplication application;

        public LogAnalyzerApplicationVM(LogAnalyzerApplication application)
        {
            this.application = application;

            logFilesVM = application.LogFiles.Select(l => new LogFileVM(l)).ToList();
            configFile = application.ConfigFile;

            selectedLog = logFilesVM[0];
            selectedLog.PropertyChanged += onChangeTabType;
            selectedLog.selectFirstTabType();   
        }

        private void onChangeTabType(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedTabType")
            {
                string selTabType = selectedLog.SelectedTabType;
                block0SeriesUp = calcBlock0SeriesUp(selTabType);
                block0SeriesBottom = calcBlock0SeriesBottom(selTabType);
                OnPropertyChanged("Block0SeriesUp");
                OnPropertyChanged("block0SeriesBottom");
            }
        }

        private SeriesCollection calcBlock0SeriesUp(string tabType)
        {
            return calcBlock0Series(tabType, new string[] { configFile.Block0Params[1] });
        }

        private SeriesCollection calcBlock0SeriesBottom(string tabType)
        {
            return calcBlock0Series(tabType,
                    new string[] { configFile.Block0Params[2], configFile.Block0Params[3], configFile.Block0Params[4] });
        }

        private SeriesCollection calcBlock0Series(string tabType, string[] configParams)
        {
            SeriesCollection result = new SeriesCollection();

            foreach (string configParam in configParams)
            {
                int[] values = logFilesVM.Select(l => l.DataBlock0.getDataLine(tabType).getValue(configParam).Value).ToArray();
                Dictionary<int, int> countValues = values.GroupBy(x => x)
                  .ToDictionary(x => x.Key, y => y.Count());
                int[] sortKeys = countValues.Keys.ToArray();
                Array.Sort(sortKeys);
                ChartValues<ObservablePoint> points =
                    new ChartValues<ObservablePoint>(sortKeys.Select(k => new ObservablePoint(k, countValues[k])).ToArray());

                LineSeries newLineSeries = new LineSeries
                {
                    Values = points,
                    Title = configParam,
                    PointGeometry = DefaultGeometries.Diamond,
                    Fill = Brushes.Transparent,
                };
                result.Add(newLineSeries);
            }

            return result;
        }

        /*private void onChangeTabType()
        {

        }*/

        //bool flag = false;

        /*public SeriesCollection SeriesTest
        {
            get
            {
                return seriesTest;
            }
            set
            {
                seriesTest = value;
                OnPropertyChanged("SeriesTest");
                //OnPropertyChanged("SeriesTest2");
            }
        }

        public SeriesCollection SeriesTest2
        {
            get
            {
                return seriesTest2;
            }
            set
            {
                seriesTest2 = value;
                OnPropertyChanged("SeriesTest2");
            }
        }*/

        /*public ObservableCollection<LogFile> Logs
        {
            get { return logFiles; }
        }*/
        System.Drawing.Color ccc;

        /*public LogFile SelectedLog
{
get { return selectedLog; }
set
{
selectedLog = value;
OnPropertyChanged("SelectedLog");
Test();
Test2();

System.Drawing.Color[] colorArray = new System.Drawing.Color[19];
for (int hue = 0; hue < 19; hue++)
{
colorArray[hue] = (System.Drawing.Color)new HSLColor(hue * 20, 0.25 * 360, 1.0 * 360);
}
int yyy = 5;
}
}  */

        /*public void Test2()
        {
            canvas.Children.Clear();
            Ellipse circle = new Ellipse()
            {
                Width = 100,
                Height = 100,
                Stroke = Brushes.Red,
                StrokeThickness = 6
            };
            //
            int count = selectedLog.Blocks[0].Lines.Count;
            Ellipse[] el = new Ellipse[count];
            TextBlock[] tb = new TextBlock[count];
            string[] st = selectedLog.Blocks[0].Lines.Keys.ToArray();

            for (int i = 0; i < count; i++)
            {
                el[i] = new Ellipse{
                    Width = 8,
                    Height = 8,
                    Stroke = Brushes.Red,
                    StrokeThickness = 4,

                };
                tb[i] = new TextBlock
                {
                    Text = st[i],
                };

                double x = 120 * Math.Cos(Math.PI * 2 / count * i);
                double y = 120 * Math.Sin(Math.PI * 2 / count * i);
                Canvas.SetTop(el[i], 130 + y);
                Canvas.SetLeft(el[i], 330 + x);
                
                canvas.Children.Add(el[i]);
                canvas.Children.Add(tb[i]);
                tb[i].UpdateLayout();

                if (x > 0)
                {
                    Canvas.SetTop(tb[i], 130 + y + 5);
                    Canvas.SetLeft(tb[i], 330 + x + 5);
                }
                else
                { 
                    Canvas.SetTop(tb[i], 130 + y + 5);
                    Canvas.SetLeft(tb[i], 330 + x - tb[i].ActualWidth - 5);
                }
            }

            count = selectedLog.Blocks[1].Lines.Count;
            int i1 = 0, i2 = 0;
            Line[] ln = new Line[count];
            int iii = 0;
            int max = 0;
            foreach (KeyValuePair<string, DataLine> cur in selectedLog.Blocks[1].Lines)
            {
                if (cur.Value.Values["Number"].Value > max)
                {
                    max = cur.Value.Values["Number"].Value;
                }
            }
            foreach (string cur in selectedLog.Blocks[1].Lines.Keys)
            {

                for (int j = 0; j < st.Length; j++)
                {
                    if (cur == st[j])
                    {
                        i1 = j;
                        break;
                    }
                }
                
                for (int j = 0; j < st.Length; j++)
                {
                    if (selectedLog.Blocks[1].Lines[cur].Values["To"].StringValue == st[j])
                    {
                        i2 = j;
                        break;
                    }
                }
                ccc = System.Drawing.ColorTranslator.FromWin32(ColorHLSToRGB(0, 120, 240));
                ln[iii] = new Line
                {
                    X1 = Canvas.GetLeft(el[i1]) + 4,
                    Y1 = Canvas.GetTop(el[i1]) + 4,
                    X2 = Canvas.GetLeft(el[i2]) + 4,
                    Y2 = Canvas.GetTop(el[i2]) + 4,
                    StrokeThickness = 2,
                    Stroke = new SolidColorBrush(Color.FromRgb(ccc.R, ccc.G, ccc.B)),
                };
                double x1 = Canvas.GetLeft(el[i1]) + 4;
                double y1 = Canvas.GetTop(el[i1]) + 4;
                double x2 = Canvas.GetLeft(el[i2]) + 4;
                double y2 = Canvas.GetTop(el[i2]) + 4;

                double len = Math.Sqrt(Math.Pow(Math.Abs(x2 - x1), 2) + Math.Pow(Math.Abs(y2 - y1), 2));
                double alpha = Math.Acos((x2 - x1) / len);
                if (y1 - y2 < 0 )
                {
                    alpha = Math.PI * 2 - alpha;
                }

                canvas.Children.Add(ln[iii]);
                iii++;
            }
        }*/

        private void chatter_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            /*if (e.PropertyName == "SelectedTabType")
                Test();*/
        }

        /*public void Test()
        {
            if (flag)
                return;
            flag = true;
            string s = "OpeningCount";// d.Key;
            string s2 = "";// selectedLog.SelectedTabType;
            
            List<int> test2 = new List<int>();
            int[] values = logFiles.Select(l => l.Blocks[0].Lines[s2].Values[s].Value).ToArray();

            int[] valuesF = logFiles.Select(l => l.Blocks[0].Lines[s2].Values["FirstTime"].Value).ToArray();
            int[] valuesL = logFiles.Select(l => l.Blocks[0].Lines[s2].Values["LongestTime"].Value).ToArray();
            int[] valuesT = logFiles.Select(l => l.Blocks[0].Lines[s2].Values["TotalTime"].Value).ToArray();

            Dictionary<int, int> query = values.GroupBy(x => x)
              .ToDictionary(x => x.Key, y => y.Count());

            Dictionary<int, int> queryF = valuesF.GroupBy(x => x)
              .ToDictionary(x => x.Key, y => y.Count());
            Dictionary<int, int> queryL = valuesL.GroupBy(x => x)
              .ToDictionary(x => x.Key, y => y.Count());
            Dictionary<int, int> queryT = valuesT.GroupBy(x => x)
              .ToDictionary(x => x.Key, y => y.Count());

            int[] keys = query.Keys.ToArray();
            int[] keysF = queryF.Keys.ToArray();
            int[] keysL = queryL.Keys.ToArray();
            int[] keysT = queryT.Keys.ToArray();

            Array.Sort(keys);
            Array.Sort(keysF);
            Array.Sort(keysL);
            Array.Sort(keysT);

            int[] valuesSort = keys.Select(k => Convert.ToInt32(query[k])).ToArray();
            int[] valuesSortF = keysF.Select(k => Convert.ToInt32(queryF[k])).ToArray();
            int[] valuesSortL = keysL.Select(k => Convert.ToInt32(queryL[k])).ToArray();
            int[] valuesSortT = keysT.Select(k => Convert.ToInt32(queryT[k])).ToArray();

            double[] keysDec = keys.Select(k => Convert.ToDouble(k)).ToArray();
            double[] keysDecF = keysF.Select(k => Convert.ToDouble(k)).ToArray();
            double[] keysDecL = keysL.Select(k => Convert.ToDouble(k)).ToArray();
            double[] keysDecT = keysT.Select(k => Convert.ToDouble(k)).ToArray();

            ChartValues<ObservablePoint> points = new ChartValues<ObservablePoint>();
            ChartValues<ObservablePoint> pointsF = new ChartValues<ObservablePoint>();
            ChartValues<ObservablePoint> pointsL = new ChartValues<ObservablePoint>();
            ChartValues<ObservablePoint> pointsT = new ChartValues<ObservablePoint>();
            for (int i = 0; i < keys.Count(); i++)
            {
                points.Add(new ObservablePoint(keys[i], query[keys[i]]));
            }
            for (int i = 0; i < keysF.Count(); i++)
            {
                pointsF.Add(new ObservablePoint(keysF[i], queryF[keysF[i]]));
            }
            for (int i = 0; i < keysL.Count(); i++)
            {
                pointsL.Add(new ObservablePoint(keysL[i], queryL[keysL[i]]));
            }
            for (int i = 0; i < keysT.Count(); i++)
            {
                pointsT.Add(new ObservablePoint(keysT[i], queryT[keysT[i]]));
            }

            int cur = selectedLog.Blocks[0].Lines[s2].Values["OpeningCount"].Value;

            int curF = selectedLog.Blocks[0].Lines[s2].Values["FirstTime"].Value;
            int curL = selectedLog.Blocks[0].Lines[s2].Values["LongestTime"].Value;
            int curT = selectedLog.Blocks[0].Lines[s2].Values["TotalTime"].Value;

            SeriesTest = new SeriesCollection
            {
                new LineSeries
                {
                    Values = points,
                    Title = "OpeningCount",
                    PointGeometry = DefaultGeometries.Diamond,
                    //DataLabels = true,
                    //Stroke = Brushes.Red,
                    Fill = Brushes.Transparent,
                },

                new ColumnSeries
                {
                    Values = new ChartValues<ObservablePoint>{ new ObservablePoint(cur, query[cur]) },
                    MaxColumnWidth = 10,
                    Fill = Brushes.Red,
                    Title = "Current Value",

                }
            };

            SeriesTest2 = new SeriesCollection
            {
                new LineSeries
                {
                    Values = pointsF,
                    Title = "FirstTime",
                    PointGeometry = DefaultGeometries.Diamond,
                    DataLabels = true,
                    Stroke = Brushes.Green,
                    Fill = Brushes.Transparent
                },
                new ColumnSeries
                {
                    Values = new ChartValues<ObservablePoint>{ new ObservablePoint(curF, queryF[curF]) },
                    MaxColumnWidth = 10,
                    Fill = Brushes.Green,
                    Title = "Current Value"
                },
                new LineSeries
                {
                    Values = pointsL,
                    Title = "LongestTime",
                    PointGeometry = DefaultGeometries.Diamond,
                    Fill = Brushes.Transparent,
                    Stroke = Brushes.Blue,
                },
                new ColumnSeries
                {
                    Values = new ChartValues<ObservablePoint>{ new ObservablePoint(curL, queryL[curL]) },
                    MaxColumnWidth = 10,
                    Fill = Brushes.Blue,
                    Title = "Current Value"

                },
                new LineSeries
                {
                    Values = pointsT,
                    Title = "TotalTime",
                    PointGeometry = DefaultGeometries.Diamond,
                    Stroke = Brushes.Red,
                    Fill = Brushes.Transparent,
                },
                new ColumnSeries
                {
                    Values = new ChartValues<ObservablePoint>{ new ObservablePoint(curT, queryT[curT]) },
                    MaxColumnWidth = 10,
                    Fill = Brushes.Red,
                    Title = "Current Value"
                }
            };
            chart.Series = SeriesTest;
            chart2.Series = SeriesTest2;
            
        }*/

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}