using PhysHelper.Enums;

namespace PhysHelper.SIObjects
{
    public abstract class Vector : ISIObject
    {
        private VectorQuantity? direction;

        private double AngleInRadians
        {
            get
            {
                return (Math.PI / 180) * Angle;
            }
        }

        public double Quantity { get; set; }

        public double Angle { get; set; }

        public VectorQuantity Direction
        {
            get
            {
                if (direction == null)
                {
                    var xComp = Quantity * Math.Cos(AngleInRadians);
                    var yComp = Quantity * Math.Sin(AngleInRadians);

                    direction = new VectorQuantity(xComp, yComp);
                }

                return direction;
            }
        }

        public double Magnitude
        {
            get
            {
                return Math.Sqrt(Math.Pow(Direction.X, 2) + Math.Pow(Direction.Y, 2));
            }
        }

        public abstract string UnitOfMeasure { get; }

        public SIState SIState { get; protected set; }

        public class VectorQuantity
        {
            public double X { get; set; }

            public double Y { get; set; }

            public VectorQuantity(double x, double y)
            {
                X = x;
                Y = y;
            }
        }
    }
}

