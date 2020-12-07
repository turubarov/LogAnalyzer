using LogAnalyzer.Model;
using System.Windows;

namespace LogAnalyzer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LogAnalyzerApplication application;
        private LogAnalyzerApplicationVM applicationVM;

        public MainWindow()
        {
            InitializeComponent();
            application = new LogAnalyzerApplication();
            applicationVM = new LogAnalyzerApplicationVM(application);
            DataContext = applicationVM;
        }
    }
}
