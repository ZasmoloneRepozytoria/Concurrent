using System.ComponentModel;

namespace Concurrent.Data
{
    public class Ball : INotifyPropertyChanged
    {
        private double _positionX;
        public double PositionX
        {
            get { return _positionX; }
            set
            {
                if (_positionX != value)
                {
                    _positionX = value;
                    OnPropertyChanged(nameof(PositionX));
                }
            }
        }

        private double _positionY;
        public double PositionY
        {
            get { return _positionY; }
            set
            {
                if (_positionY != value)
                {
                    _positionY = value;
                    OnPropertyChanged(nameof(PositionY));
                }
            }
        }

        private double _radius;
        public double Radius
        {
            get { return _radius; }
            set
            {
                if (_radius != value)
                {
                    _radius = value;
                    OnPropertyChanged(nameof(Radius));
                }
            }
        }

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
