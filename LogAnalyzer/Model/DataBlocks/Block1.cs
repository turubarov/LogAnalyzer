using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAnalyzer.Model.DataBlocks
{
    class Block1
    {
        private int[,] countOfTransitions;
        private string[] tabTypes;

        public Block1(int[,] countOfTransitions, string[] tabTypes)
        {
            this.countOfTransitions = countOfTransitions;
            this.tabTypes = tabTypes;
        }
    }
}
