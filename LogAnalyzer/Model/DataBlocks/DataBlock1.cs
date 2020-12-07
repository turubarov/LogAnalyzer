namespace LogAnalyzer.Model.DataBlocks
{
    public class DataBlock1
    {
        private int[,] countOfTransitions;
        public int[,] CountOfTransitions { get { return countOfTransitions;  } }

        private string[] tabTypes;

        public DataBlock1(int[,] countOfTransitions, string[] tabTypes)
        {
            this.countOfTransitions = countOfTransitions;
            this.tabTypes = tabTypes;
        }

        public int getCountOfTransition(string tabFirst, string tabSecond)
        {
            int tabFirstIndex = 0;
            int tabSecondIndex = 0;
            for (int i = 0; i < tabTypes.Length; i++)
            {
                if (tabTypes[i] == tabFirst)
                    tabFirstIndex = i;
                if (tabTypes[i] == tabSecond)
                    tabSecondIndex = i;
                if (tabFirstIndex != 0 && tabSecondIndex != 0)
                    break;
            }
            return countOfTransitions[tabFirstIndex, tabSecondIndex];
        }
    }
}
