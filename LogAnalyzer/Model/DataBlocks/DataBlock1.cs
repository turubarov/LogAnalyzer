namespace LogAnalyzer.Model.DataBlocks
{
    public class DataBlock1
    {
        private int[,] countOfTransitions;
        private string[] tabTypes;

        public DataBlock1(int[,] countOfTransitions, string[] tabTypes)
        {
            this.countOfTransitions = countOfTransitions;
            this.tabTypes = tabTypes;
        }
    }
}
