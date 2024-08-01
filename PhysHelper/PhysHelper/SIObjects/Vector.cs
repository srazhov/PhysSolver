using PhysHelper.Enums;

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

        private double GetAngleInRadians()
        {
            return Angle * Math.PI / 180;
        }

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
                    var xComp = Math.Round(Quantity * Math.Cos(GetAngleInRadians()), 5);
                    var yComp = Math.Round(Quantity * Math.Sin(GetAngleInRadians()), 5);

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

        public class VectorQuantity(double x, double y)
        {
            public double X { get; set; } = x;

            public double Y { get; set; } = y;
        }
    }
}

