using Concurrent.Presentation.ViewModel;
using System.Windows;

namespace Concurrent
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ViewModel _viewModel = new ViewModel();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void CreateBalls_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.CreateBalls_Click(sender, e); // Forward the event to the ViewModel
        }
    }
}