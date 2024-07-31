using PhysHelper.Enums;

namespace PhysHelper.SIObjects.Scalars
{
    public abstract class Scalar : ISIObject
    {
        public abstract string UnitOfMeasure { get; }

        public SIState SIState { get; set; }

        public double Quantity { get; set; }

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

