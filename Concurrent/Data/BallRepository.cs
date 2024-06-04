namespace Concurrent.Data
{
    public class BallRepository
    {
        private readonly List<Ball> _balls;

        public BallRepository()
        {
            _balls = new List<Ball>();
        }

        public IEnumerable<Ball> GetBalls()
        {
            return _balls.ToList();
        }

        public void AddBall(Ball ball)
        {
            _balls.Add(ball);
        }

        public void RemoveBall(Ball ball)
        {
            _balls.Remove(ball);
        }
    }
}
