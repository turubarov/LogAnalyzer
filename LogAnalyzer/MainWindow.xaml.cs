//using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace LogAnalyzer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ApplicationViewModel avm;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ApplicationViewModel(Chart, Chart2, MyCanvas);
            avm = (ApplicationViewModel)DataContext;
        }
    }
}
