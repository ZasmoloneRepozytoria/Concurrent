namespace Concurrent.Data
{
    public class Ball
    {
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public double Radius { get; set; }

        public Ball(double pozycjaX, double pozycjaY, double promien)
        {
            PositionX = pozycjaX;
            PositionY = pozycjaY;
            Radius = promien;
        }

        public void UpdatePosition(double newPositionX, double newPositionY)
        {
            PositionX = newPositionX;
            PositionY = newPositionY;
        }
    }
}
