using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using Model;
using Concurrent.Data;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Shapes;

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
        public ViewModel() {
            _ballRepository = new BallRepository();
            Balls = new ObservableCollection<Ball>();
        }
        public void CreateBalls_Click(object sender, RoutedEventArgs e)
        {
            // Parse the amount of balls from the text box
            if (int.TryParse((Application.Current.MainWindow.FindName("txtAmount") as TextBox)?.Text, out int amount))
            {
                Model.Model.createBalls(amount, 30, _ballRepository);
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
