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

        public LogFileVM(LogFile logFile)
        {
            this.logFile = logFile;
            this.tabTypes = logFile.DataBlock0.getTabTypes();
        }

        public void selectFirstTabType()
        {
            SelectedTabType = tabTypes[0];
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
