using PhysHelper.Enums;

namespace PhysHelper.SIObjects.Scalars
{
    public abstract class Scalar : ISIObject
    {
        private double quantity;

        public abstract string UnitOfMeasure { get; }

        public SIState SIState { get; set; }

        public double Quantity
        {
            get => SIState == SIState.Known ? quantity : double.NaN;
            set => quantity = value;
        }

        public Scalar(double quantity)
        {
            Quantity = quantity;
            SIState = SIState.Known;
        }

        public Scalar()
        {
            SIState = SIState.Unimportant;
        }
    }
}

