using LogAnalyzer.Model;
using LogAnalyzer.Model.DataBlocks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LogAnalyzer.ViewModel
{
    public class LogFileVM : INotifyPropertyChanged
    {
        private LogFile logFile;

        public DataBlock0 DataBlock0 { get { return logFile.DataBlock0; } }
        public DataBlock1 DataBlock1 { get { return logFile.DataBlock1; } }
        public DataBlock2 DataBlock2 { get { return logFile.DataBlock2; } }
        public string FileName { get { return logFile.FileName; } }

        private string[] tabTypes;
        public string[] TabTypes { get { return tabTypes; } }

        private string[] tabTypes2;
        public string[] TabTypes2 { get { return tabTypes2; } }

        private string selectedTabType;
        public string SelectedTabType
        {
            get
            {
                return selectedTabType;
            }
            set
            {
                string old = selectedTabType;
                selectedTabType = value;
                selectedDataLine = logFile.DataBlock0.getDataLine(selectedTabType);
                OnPropertyChanged("SelectedTabType");
                OnPropertyChanged("SelectedDataLine");
            }
        }

        private DataLine selectedDataLine;
        public DataLine SelectedDataLine { get { return selectedDataLine;  } }

        private string selectedTabType2;
        public string SelectedTabType2
        {
            get
            {
                return selectedTabType2;
            }
            set
            {
                string old = selectedTabType2;
                selectedTabType2 = value;
                selectedDataLine2 = logFile.DataBlock2.getDataLine(selectedTabType2);
                OnPropertyChanged("SelectedTabType2");
                OnPropertyChanged("SelectedDataLine2");
            }
        }

        private DataLine selectedDataLine2;
        public DataLine SelectedDataLine2 { get { return selectedDataLine2; } }

        public LogFileVM(LogFile logFile)
        {
            this.logFile = logFile;
            this.tabTypes = logFile.DataBlock0.getTabTypes();
            this.tabTypes2 = logFile.DataBlock2.getTabTypes();
        }

        public void selectFirstTabType()
        {
            SelectedTabType = tabTypes[0];
        }

        public void selectFirstTabType2()
        {
            SelectedTabType2 = tabTypes2[0];
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
