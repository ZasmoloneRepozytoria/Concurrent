using System;
using System.IO;
using System.Threading.Tasks;
using Concurrent.Data;
using Newtonsoft.Json;

namespace Concurrent.Logic
{
    public class DiagnosticLogger
    {
        private readonly string _filePath;
        private readonly object _fileLock = new object();

        public DiagnosticLogger(string filePath)
        {
            _filePath = filePath;
        }

        public async Task LogAsync(string message)
        {
            lock (_fileLock)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(_filePath, true))
                    {
                        writer.WriteLine(message);
                    }
                }
                catch (IOException)
                {
                    // Handle file I/O issues, e.g., retry logic, log to a different medium, etc.
                }
            }
        }

        public async Task LogBallStateAsync(Ball ball)
        {
            var ballState = new
            {
                PositionX = ball.PositionX,
                PositionY = ball.PositionY,
                Radius = ball.Radius,
                VelocityX = ball.VelocityX,
                VelocityY = ball.VelocityY,
                Timestamp = DateTime.UtcNow
            };

            string json = JsonConvert.SerializeObject(ballState);
            await LogAsync(json);
        }
    }
}
