﻿using Concurrent.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concurrent.Logic
{
    // Implementation of the ball logic
    public class BallLogic 
    {
        private readonly BallRepository _ballRepository;
        private readonly List<Thread> _ballThreads;

        public BallLogic(BallRepository ballRepository)
        {
            _ballRepository = ballRepository;
            _ballThreads = new List<Thread>();
        }

        public void CreateBall(double positionX, double positionY, double radius)
        {
            // Create a new ball
            Ball newBall = new Ball(positionX, positionY, radius);

            // Add the new ball to the repository
            _ballRepository.AddBall(newBall);

            // Start a new thread to simulate movement for the new ball
            Thread ballThread = new Thread(() => SimulateBallMovement(newBall));
            _ballThreads.Add(ballThread);
            ballThread.Start();
        }

        private void SimulateBallMovement(Ball ball)
        {
            Random random = new Random();

            while (true)
            {
                // Simulate movement by updating ball position
                double deltaX = random.NextDouble() * 10 - 5; // Random value between -5 and 5
                double deltaY = random.NextDouble() * 10 - 5; // Random value between -5 and 5

                // Update ball position
                lock (ball) // Ensure thread-safe access to ball position
                {
                    ball.PositionX += deltaX;
                    ball.PositionY += deltaY;
                }

                // Sleep for a short duration to simulate periodic updates
                Thread.Sleep(100); // Sleep for 1 second
            }
        }
    }
}