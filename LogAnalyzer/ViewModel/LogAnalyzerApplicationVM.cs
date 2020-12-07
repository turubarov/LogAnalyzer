using System.ComponentModel;
using LogAnalyzer.Model;
using System.Collections.Generic;
using System.Linq;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using LiveCharts.Defaults;
using System.Windows.Media;
using System.Runtime.InteropServices;
using LogAnalyzer.ViewModel;
using System.Runtime.CompilerServices;

namespace LogAnalyzer
{
    public class LogAnalyzerApplicationVM : INotifyPropertyChanged
    {
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
                selectedLog.PropertyChanged += onChangeTabType;
                OnPropertyChanged("SelectedLog");
                OnPropertyChanged("RedrawGraph");
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
            OnPropertyChanged("RedrawGraph");
        }

        private void onChangeTabType(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedTabType")
            {
                string selTabType = selectedLog.SelectedTabType;
                block0SeriesUp = calcBlock0SeriesUp(selTabType);
                block0SeriesBottom = calcBlock0SeriesBottom(selTabType);
                OnPropertyChanged("Block0SeriesUp");
                OnPropertyChanged("Block0SeriesBottom");
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

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        
        System.Drawing.Color ccc;

        /*

System.Drawing.Color[] colorArray = new System.Drawing.Color[19];
for (int hue = 0; hue < 19; hue++)
{
colorArray[hue] = (System.Drawing.Color)new HSLColor(hue * 20, 0.25 * 360, 1.0 * 360);
}
int yyy = 5;
}
}  */

        
    }
}