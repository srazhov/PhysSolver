using PhysHelper.SIObjects.Enums;

namespace PhysHelper.SIObjects
{
    public abstract class Scalar : ISIObject
    {
        public abstract string UnitOfMeasure { get; }

        public SIState SIState { get; }
    }
}

