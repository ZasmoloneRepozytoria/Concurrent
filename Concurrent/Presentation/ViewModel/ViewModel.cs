using Concurrent.Data;
using Concurrent.Logic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Concurrent.Presentation.ViewModel
{

    public class BallModelView
    {
        private Ball _ball;
        BallModelView(Ball ball)
        {
            _ball = ball;
        }


    }

    public class ViewModel
    {
        private BallRepository _ballRepository;
        public ObservableCollection<Ball> Balls { get; set; }
        private DiagnosticLogger _logger;
        public ViewModel()
        {
            _ballRepository = new BallRepository();
            Balls = new ObservableCollection<Ball>();
            _logger = new DiagnosticLogger("C:\\Users\\Public\\ball_simulation_log.txt");
        }
        public void CreateBalls_Click(object sender, RoutedEventArgs e)
        {
            // Parse the amount of balls from the text box
            if (int.TryParse((Application.Current.MainWindow.FindName("txtAmount") as TextBox)?.Text, out int amount))
            {
                Model.Model.createBalls(amount, 30, _ballRepository, _logger);
                var ballsFromRepo = _ballRepository.GetBalls();
                foreach (var ball in ballsFromRepo)
                {
                    Balls.Add(ball);
                }
            }
            else
            {
                MessageBox.Show("Invalid input for amount of balls.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
