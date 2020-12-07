using System.ComponentModel;
using System.Runtime.CompilerServices;
using LogAnalyzer.Model.DataBlocks;

namespace LogAnalyzer.Model
{
    public class LogFile
    { 
        // название файла лога
        private string fileName;
        public string FileName { get { return fileName; } }

        // блоки данных
        private DataBlock0 dataBlock0;
        public DataBlock0 DataBlock0 { get { return dataBlock0; } }

        private DataBlock1 dataBlock1;
        public DataBlock1 DataBlock1 { get { return dataBlock1; } }

        private DataBlock2 dataBlock2;
        public DataBlock2 DataBlock2 { get { return dataBlock2; } }

        public event PropertyChangedEventHandler PropertyChanged;

        public LogFile(string fileName, DataBlock0 dataBlock0, DataBlock1 dataBlock1, DataBlock2 dataBlock2)
        {
            this.fileName = fileName;
            this.dataBlock0 = dataBlock0;
            this.dataBlock1 = dataBlock1;
            this.dataBlock2 = dataBlock2;
        }

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
