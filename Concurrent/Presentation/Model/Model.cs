using Concurrent;
using Concurrent.Data;
using Concurrent.Logic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Model
{
    public class Model
    {
        public static void createBalls(int value, int radius, BallRepository repository)
        {
            BallLogic ballLogic = new BallLogic(repository);
        Random random = new Random();
            for (int i=0; i<value; i++)
            {
                int x = random.Next(50, 350);
                int y = random.Next(50, 350);
                ballLogic.CreateBall(x, y, radius);
            }
        }
    }

}