using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using LogAnalyzer.Model.DataBlocks;

namespace LogAnalyzer.Model
{
    public class LogFile : INotifyPropertyChanged
    {
        // путь до директории с логами
        private string filePath;

        // название файла лога
        private string fileName;


        private string[,] maskParams;
        private string[] lineParams;

        // блоки данных
        private Block0 block0;
        private Block1 block1;
        private Block2 block2;

        private List<MyBlock> blocks;

        public string[,] MaskParams
        {
            get
            {
                return maskParams;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public LogFile(string filePath, List<MyBlock> blocks, string[,] maskParams, string[] lineParams)
        {
            this.filePath = filePath;
            this.blocks = blocks;
            this.maskParams = maskParams;
            this.lineParams = lineParams;
            fileName = Path.GetFileName(filePath);
        }

        private string readFile()
        {
            return File.ReadAllText(filePath);
        }

        //private List<>

        public List<MyBlock> Blocks
        {
            get { return blocks; }
        }

        private string selectedTabType;
        public string SelectedTabType
        {
            get { return selectedTabType; }
            set
            {
                selectedTabType = value;
                curLine = Blocks[0].Lines[selectedTabType].Value;
                OnPropertyChanged("SelectedTabType");
                OnPropertyChanged("CurLine");
            }
        }

        public void SelectFirstItem()
        {
            int i = 3;
            i += 2;
            //selectedTabType = Blocks[0].Lines[mas];
        }

        private Dictionary<string, Data> curLine;
        public Dictionary<string, Data> CurLine
        {
            get
            {
                return curLine;
            }
            set
            {
                curLine = value;
                OnPropertyChanged("CurLine");
            }
        }

        public List<string> CurrentTabTypes
        {
            get
            {
                List<string> l = new List<string>(Blocks[0].Lines.Keys);
                SelectedTabType = lineParams[0];
                return l;
            }
        }

        public string FilePath
        {
            get { return filePath; }
            set
            {
                filePath = value;
                OnPropertyChanged("FilePath");
            }
        }

        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                OnPropertyChanged("FileName");
            }
        }

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
