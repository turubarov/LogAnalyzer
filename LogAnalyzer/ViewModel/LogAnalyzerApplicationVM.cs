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

                selectedLog.PropertyChanged += onChangeTabType;
                OnPropertyChanged("SelectedLog");
                OnPropertyChanged("RedrawGraph");
            }
        }

        private SeriesCollection block0SeriesUp;
        public SeriesCollection Block0SeriesUp { get { return block0SeriesUp; } }

        private SeriesCollection block0SeriesBottom;
        public SeriesCollection Block0SeriesBottom { get { return block0SeriesBottom; } }

        private SeriesCollection pieBlock0Series;
        public SeriesCollection PieBlock0Series { get { return pieBlock0Series; } }

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

            pieBlock0Series = calcPieBlock0Series();
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
                pieBlock0Series = calcPieBlock0Series();
                OnPropertyChanged("Block0SeriesUp");
                OnPropertyChanged("Block0SeriesBottom");
                OnPropertyChanged("PieBlock0Series");
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

        private SeriesCollection calcPieBlock0Series()
        {
            return new SeriesCollection
            {
                new PieSeries
                {
                    Title = configFile.Block0Params[2],
                    Values = new ChartValues<int> {selectedLog.DataBlock0.getDataLine(selectedLog.SelectedTabType).Values[configFile.Block0Params[2]].Value},
                    DataLabels = true,
                },
                new PieSeries
                {
                    Title = configFile.Block0Params[3],
                    Values = new ChartValues<int> {selectedLog.DataBlock0.getDataLine(selectedLog.SelectedTabType).Values[configFile.Block0Params[3]].Value},
                    DataLabels = true,
                },
                new PieSeries
                {
                    Title = configFile.Block0Params[4],
                    Values = new ChartValues<int> {selectedLog.DataBlock0.getDataLine(selectedLog.SelectedTabType).Values[configFile.Block0Params[4]].Value},
                    DataLabels = true,
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