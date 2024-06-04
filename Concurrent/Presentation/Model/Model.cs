using Concurrent.Data;
using Concurrent.Logic;

namespace Model
{
    public class Model
    {
        public static void createBalls(int value, int radius, BallRepository repository, DiagnosticLogger logger)
        {
            BallLogic ballLogic = new BallLogic(repository, logger);
            Random random = new Random();
            for (int i = 0; i < value; i++)
            {
                int x = random.Next(50, 350);
                int y = random.Next(50, 350);
                ballLogic.CreateBall(x, y, radius);
            }
        }
    }

}