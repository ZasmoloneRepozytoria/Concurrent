using Concurrent.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Concurrent.Logic
{
    // Implementation of the ball logic
    public class BallLogic
    {
        private readonly BallRepository _ballRepository;
        private readonly List<Task> _ballTasks;
        private readonly object _ballLock = new object();
        private readonly Random _random = new Random();

        public BallLogic(BallRepository ballRepository)
        {
            _ballRepository = ballRepository;
            _ballTasks = new List<Task>();
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

            // Start a new task to simulate movement for the new ball
            Task ballTask = Task.Run(() => SimulateBallMovementAsync(newBall));
            _ballTasks.Add(ballTask);
        }

        private async Task SimulateBallMovementAsync(Ball ball)
        {
            while (true)
            {
                lock (_ballLock) // Ensure thread-safe access to all balls
                {
                    // Update ball position
                    ball.PositionX += ball.VelocityX;
                    ball.PositionY += ball.VelocityY;

                    // Check for collision with walls
                    if (ball.PositionX >= 450 + ball.Radius/2 || ball.PositionX <= 0)
                    {
                        ball.VelocityX = -ball.VelocityX;
                       // ball.PositionX = Math.Clamp(ball.PositionX, ball.Radius, 450 - ball.Radius);
                    }

                    if (ball.PositionY >= 450 + ball.Radius/2 || ball.PositionY <= 0)
                    {
                        ball.VelocityY = -ball.VelocityY;
                       // ball.PositionY = Math.Clamp(ball.PositionY, ball.Radius, 450 - ball.Radius);
                    }

                    // Check for collision with other balls
                    foreach (var otherBall in _ballRepository.GetBalls().Where(b => b != ball))
                    {
                        if (IsColliding(ball, otherBall))
                        {
                            ResolveCollision(ball, otherBall);
                        }
                    }
                }

                // Delay for a short duration to simulate periodic updates
                await Task.Delay(10);
            }
        }

        private bool IsColliding(Ball ball1, Ball ball2)
        {
            double dx = ball1.PositionX - ball2.PositionX;
            double dy = ball1.PositionY - ball2.PositionY;
            double distance = Math.Sqrt(dx * dx + dy * dy);
            return distance < ball1.Radius + ball2.Radius/100;
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
