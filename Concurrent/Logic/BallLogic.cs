using Concurrent.Data;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Concurrent.Logic
{
    public class BallLogic
    {
        private readonly BallRepository _ballRepository;
        private readonly DiagnosticLogger _logger;
        private readonly object _ballLock = new object();
        private readonly Random _random = new Random();
        private readonly System.Timers.Timer _updateTimer;

        public BallLogic(BallRepository ballRepository, DiagnosticLogger logger)
        {
            _ballRepository = ballRepository;
            _logger = logger;
            _updateTimer = new Timer(20); // Update interval in milliseconds
            _updateTimer.Elapsed += UpdateTimer_Elapsed;
            _updateTimer.AutoReset = true;
            _updateTimer.Start();
        }

        public void CreateBall(double positionX, double positionY, double radius)
        {
            // Create a new ball with random initial velocity
            Ball newBall = new Ball(positionX, positionY, radius)
            {
                VelocityX = (_random.NextDouble() - 0.5) * 10, // Random value between -5 and 5
                VelocityY = (_random.NextDouble() - 0.5) * 10  // Random value between -5 and 5
            };

            // Add the new ball to the repository
            _ballRepository.AddBall(newBall);
        }

        private void UpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (_ballLock)
            {
                // Update ball positions and check for collisions
                foreach (var ball in _ballRepository.GetBalls())
                {
                    ball.PositionX += ball.VelocityX;
                    ball.PositionY += ball.VelocityY;

                    // Check for collision with walls
                    if (ball.PositionX >= 450 - ball.Radius || ball.PositionX <= ball.Radius)
                    {
                        ball.VelocityX = -ball.VelocityX;
                        ball.PositionX = Math.Clamp(ball.PositionX, ball.Radius, 450 - ball.Radius);
                    }

                    if (ball.PositionY >= 450 - ball.Radius || ball.PositionY <= ball.Radius)
                    {
                        ball.VelocityY = -ball.VelocityY;
                        ball.PositionY = Math.Clamp(ball.PositionY, ball.Radius, 450 - ball.Radius);
                    }

                    // Check for collision with other balls
                    foreach (var otherBall in _ballRepository.GetBalls().Where(b => b != ball))
                    {
                        if (IsColliding(ball, otherBall))
                        {
                            ResolveCollision(ball, otherBall);
                            _ = Task.Run(async () => await _logger.LogBallStateAsync(ball));
                        }
                    }
                }
            }
        }

        private bool IsColliding(Ball ball1, Ball ball2)
        {
            double dx = ball1.PositionX - ball2.PositionX;
            double dy = ball1.PositionY - ball2.PositionY;
            double distance = Math.Sqrt(dx * dx + dy * dy);
            return distance < (ball1.Radius + ball2.Radius)/2;
        }

        private void ResolveCollision(Ball ball1, Ball ball2)
        {
            // Calculate the normal vector
            double dx = ball1.PositionX - ball2.PositionX;
            double dy = ball1.PositionY - ball2.PositionY;
            double distance = Math.Sqrt(dx * dx + dy * dy);

            if (distance == 0) return; // Avoid division by zero

            double nx = dx / distance;
            double ny = dy / distance;

            // Calculate the relative velocity
            double relativeVelocityX = ball1.VelocityX - ball2.VelocityX;
            double relativeVelocityY = ball1.VelocityY - ball2.VelocityY;

            // Calculate the velocity along the normal
            double velocityAlongNormal = relativeVelocityX * nx + relativeVelocityY * ny;

            // Only resolve if the balls are moving towards each other
            if (velocityAlongNormal > 0)
                return;

            // Reflect velocities along the normal
            ball1.VelocityX -= velocityAlongNormal * nx;
            ball1.VelocityY -= velocityAlongNormal * ny;
            ball2.VelocityX += velocityAlongNormal * nx;
            ball2.VelocityY += velocityAlongNormal * ny;

            // Normalize the new velocities to preserve speed
            double speed1 = Math.Sqrt(ball1.VelocityX * ball1.VelocityX + ball1.VelocityY * ball1.VelocityY);
            double speed2 = Math.Sqrt(ball2.VelocityX * ball2.VelocityX + ball2.VelocityY * ball2.VelocityY);

            ball1.VelocityX = ball1.VelocityX * speed1 / Math.Sqrt(ball1.VelocityX * ball1.VelocityX + ball1.VelocityY * ball1.VelocityY);
            ball1.VelocityY = ball1.VelocityY * speed1 / Math.Sqrt(ball1.VelocityX * ball1.VelocityX + ball1.VelocityY * ball1.VelocityY);
            ball2.VelocityX = ball2.VelocityX * speed2 / Math.Sqrt(ball2.VelocityX * ball2.VelocityX + ball2.VelocityY * ball2.VelocityY);
            ball2.VelocityY = ball2.VelocityY * speed2 / Math.Sqrt(ball2.VelocityX * ball2.VelocityX + ball2.VelocityY * ball2.VelocityY);

            // Separate the balls to avoid overlap
            double overlap = 0.5 * (ball1.Radius + ball2.Radius - distance);
            ball1.PositionX += overlap * nx;
            ball1.PositionY += overlap * ny;
            ball2.PositionX -= overlap * nx;
            ball2.PositionY -= overlap * ny;
        }
    }
}
