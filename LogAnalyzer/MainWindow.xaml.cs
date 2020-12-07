using LogAnalyzer.Model;
using LogAnalyzer.View;
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
        private LogAnalyzerApplicationView applicationView;

        public MainWindow()
        {
            InitializeComponent();
            application = new LogAnalyzerApplication();
            applicationVM = new LogAnalyzerApplicationVM(application);
            applicationView = new LogAnalyzerApplicationView(this, applicationVM);
            DataContext = applicationVM;
        }
    }
}
