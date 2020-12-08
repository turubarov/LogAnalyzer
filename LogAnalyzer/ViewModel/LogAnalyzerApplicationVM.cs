using System.ComponentModel;
using LogAnalyzer.Model;
using System.Collections.Generic;
using System.Linq;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using LiveCharts.Defaults;
using System.Windows.Media;
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
        private string selectedTabType2;

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
                selectedTabType2 = selectedLog.SelectedTabType2;
                selectedLog = value;
                selectedLog.SelectedTabType = selectedTabType;
                selectedLog.SelectedTabType2 = selectedTabType2;

                curOpeningCount = selectedLog.DataBlock0.getDataLine(selectedTabType).getValue("OpeningCount").Value;
                curFirstTime = selectedLog.DataBlock0.getDataLine(selectedTabType).getValue("FirstTime").Value;
                curLongestTime = selectedLog.DataBlock0.getDataLine(selectedTabType).getValue("LongestTime").Value;
                curTotalTime = selectedLog.DataBlock0.getDataLine(selectedTabType).getValue("TotalTime").Value;

                selectedLog.PropertyChanged += onChangeTabType;

                stackColumnSeries = calcStackColumnSeries();

                OnPropertyChanged("CurOpeningCount");
                OnPropertyChanged("CurFirstTime");
                OnPropertyChanged("CurLongestTime");
                OnPropertyChanged("CurTotalTime");

                OnPropertyChanged("CurCalled");
                OnPropertyChanged("CurTotal");
                OnPropertyChanged("CurSpend");

                OnPropertyChanged("SelectedLog");
                OnPropertyChanged("StackColumnSeries");
                OnPropertyChanged("RedrawGraph");
            }
        }

        private int curOpeningCount;
        public int CurOpeningCount { get { return curOpeningCount; } }

        private int curFirstTime;
        public int CurFirstTime { get { return curFirstTime; } }

        private int curLongestTime;
        public int CurLongestTime { get { return curLongestTime; } }

        private int curTotalTime;
        public int CurTotalTime { get { return curTotalTime; } }


        private int curCalled;
        public int CurCalled { get { return curCalled; } }

        private int curTotal;
        public int CurTotal { get { return curTotal; } }

        private int curSpend;
        public int CurSpend { get { return curSpend; } }


        private SeriesCollection block0SeriesUp;
        public SeriesCollection Block0SeriesUp { get { return block0SeriesUp; } }

        private SeriesCollection block0SeriesBottom;
        public SeriesCollection Block0SeriesBottom { get { return block0SeriesBottom; } }

        private SeriesCollection block2SeriesUp;
        public SeriesCollection Block2SeriesUp { get { return block2SeriesUp; } }

        private SeriesCollection block2SeriesBottom;
        public SeriesCollection Block2SeriesBottom { get { return block2SeriesBottom; } }

        private SeriesCollection stackColumnSeries;
        public SeriesCollection StackColumnSeries { get { return stackColumnSeries; } }

        private LogAnalyzerApplication application;

        public LogAnalyzerApplicationVM(LogAnalyzerApplication application)
        {
            this.application = application;

            logFilesVM = application.LogFiles.Select(l => new LogFileVM(l)).ToList();
            configFile = application.ConfigFile;

            selectedLog = logFilesVM[0];
            selectedLog.PropertyChanged += onChangeTabType;
            selectedLog.selectFirstTabType();
            selectedLog.selectFirstTabType2();

            stackColumnSeries = calcStackColumnSeries();
            OnPropertyChanged("RedrawGraph");
            OnPropertyChanged("PieBlock0Series");
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

                curOpeningCount = selectedLog.DataBlock0.getDataLine(selTabType).getValue("OpeningCount").Value;
                curFirstTime = selectedLog.DataBlock0.getDataLine(selTabType).getValue("FirstTime").Value;
                curLongestTime = selectedLog.DataBlock0.getDataLine(selTabType).getValue("LongestTime").Value;
                curTotalTime = selectedLog.DataBlock0.getDataLine(selTabType).getValue("TotalTime").Value;

                OnPropertyChanged("CurOpeningCount");
                OnPropertyChanged("CurFirstTime");
                OnPropertyChanged("CurLongestTime");
                OnPropertyChanged("CurTotalTime");
            }
            if (e.PropertyName == "SelectedTabType2")
            {
                block2SeriesUp = calcBlock2SeriesUp(selectedLog.SelectedTabType2);
                block2SeriesBottom = calcBlock2SeriesBottom(selectedLog.SelectedTabType2);
                OnPropertyChanged("Block2SeriesUp");
                OnPropertyChanged("Block2SeriesBottom");

                curCalled = selectedLog.DataBlock2.getDataLine(selectedLog.SelectedTabType2).getValue("CalledInFirstOpenTab").Value;
                curTotal = selectedLog.DataBlock2.getDataLine(selectedLog.SelectedTabType2).getValue("TotalCalledCount").Value;
                curSpend = selectedLog.DataBlock2.getDataLine(selectedLog.SelectedTabType2).getValue("SpentTime").Value;

                OnPropertyChanged("CurCalled");
                OnPropertyChanged("CurTotal");
                OnPropertyChanged("CurSpend");
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

        private SeriesCollection calcBlock2SeriesUp(string tabType)
        {
            return calcBlock2Series(tabType, new string[] { configFile.Block2Params[2], configFile.Block2Params[3] });
        }

        private SeriesCollection calcBlock2SeriesBottom(string tabType)
        {
            return calcBlock2Series(tabType,
                    new string[] {configFile.Block2Params[4] });
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

        private SeriesCollection calcBlock2Series(string tabType, string[] configParams)
        {
            SeriesCollection result = new SeriesCollection();

            foreach (string configParam in configParams)
            {
                int[] values = logFilesVM.Select(l => l.DataBlock2.getDataLine(tabType).getValue(configParam).Value).ToArray();
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

        private SeriesCollection calcStackColumnSeries()
        {
            List<int> firstTimeVal = selectedLog.TabTypes.Select(t => selectedLog.DataBlock0.getDataLine(t).getValue("FirstTime").Value).ToList();
            List<int> longestTimeVal = selectedLog.TabTypes.Select(t => selectedLog.DataBlock0.getDataLine(t).getValue("LongestTime").Value).ToList();
            List<int> totalTimeVal = selectedLog.TabTypes.Select(t => selectedLog.DataBlock0.getDataLine(t).getValue("TotalTime").Value).ToList();
            return new SeriesCollection
            {
                new StackedColumnSeries
                {

                    Values = new ChartValues<int> (firstTimeVal),
                    StackMode = StackMode.Values, 
                    Title = "FirstTime"
                },
                new StackedColumnSeries
                {
                    Values = new ChartValues<int> (longestTimeVal),
                    StackMode = StackMode.Values,
                    Title = "LongestTime"
                },
                new StackedColumnSeries
                {
                    Values = new ChartValues<int> (totalTimeVal),
                    StackMode = StackMode.Values,
                    Title = "TotalTime"
                }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        } 
    }
}