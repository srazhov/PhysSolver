using PhysHelper.Enums;

namespace PhysHelper.SIObjects.Scalars
{
    public abstract class Scalar : ISIObject
    {
        public abstract string UnitOfMeasure { get; }

        public SIState SIState { get; set; }

        public double Quantity { get; set; }
    }
}

