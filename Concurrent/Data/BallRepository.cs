namespace Concurrent.Data
{
    public class BallRepository
    {
        private List<Ball> _balls;
        public BallRepository()
            {
                _balls = new List<Ball>();
            }
        public List<Ball> getBalls()
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
