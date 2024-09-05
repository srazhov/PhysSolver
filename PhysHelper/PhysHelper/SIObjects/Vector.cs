using PhysHelper.Enums;

namespace PhysHelper.SIObjects
{
    public abstract class Vector : ISIObject
    {
        public Vector(double quantity, double angle)
        {
            SIState = SIState.Known;
            Direction = new VectorQuantity(quantity, angle);
        }

        public Vector()
        {
            SIState = SIState.Unimportant;
            Direction = new VectorQuantity(double.NaN, 0);
        }

        public double Quantity
        {
            get => SIState == SIState.Known ? Direction.Quantity : double.NaN;
            protected set => Direction.Quantity = value;
        }

        public double Angle
        {
            get => Direction.Angle;
            private set => Direction.Angle = value;
        }

        public VectorQuantity Direction { get; private set; }

        public double Magnitude => Direction.Magnitude;

        public abstract string UnitOfMeasure { get; }

        public SIState SIState { get; set; }
    }
}

