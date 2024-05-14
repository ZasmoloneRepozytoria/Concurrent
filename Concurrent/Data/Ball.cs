namespace Concurrent.Data
{
    public class Ball
    {
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public double Radius { get; set; }

        public Ball(double positionX, double positionY, double radius)
        {
            PositionX = positionX;
            PositionY = positionY;
            Radius = radius;
        }

        public void UpdatePosition(double newPositionX, double newPositionY)
        {
            PositionX = newPositionX;
            PositionY = newPositionY;
        }
    }
}
