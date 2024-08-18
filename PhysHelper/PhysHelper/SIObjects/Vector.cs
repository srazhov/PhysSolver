using PhysHelper.Enums;
using PhysHelper.Helpers;

namespace PhysHelper.SIObjects
{
    public abstract class Vector : ISIObject
    {
        public Vector(double quantity, double angle)
        {
            Quantity = quantity;
            Angle = angle;
            SIState = SIState.Known;
        }

        public Vector()
        {
            SIState = SIState.Unimportant;
        }

        private VectorQuantity? direction;

        private double angle;

        public double Quantity { get; set; }

        public double Angle
        {
            get => angle;
            set => angle = Math.Abs(value % 360);
        }

        public virtual VectorQuantity Direction
        {
            get
            {
                if (direction == null)
                {
                    var xComp = Math.Round(Quantity * Math.Cos(HelperClass.GetAngleInRadians(Angle)), 5);
                    var yComp = Math.Round(Quantity * Math.Sin(HelperClass.GetAngleInRadians(Angle)), 5);

                    direction = new VectorQuantity(xComp, yComp);
                }

                return direction;
            }
        }

        public double Magnitude => Direction.Magnitude;

        public abstract string UnitOfMeasure { get; }

        public SIState SIState { get; protected set; }

        public class VectorQuantity(double x, double y)
        {
            public double X { get; set; } = x;

            public double Y { get; set; } = y;

            public double Magnitude => Math.Round(Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2)), 5);
        }
    }
}

