using PhysHelper.Enums;

namespace PhysHelper.SIObjects
{
    public interface ISIObject
    {
        string UnitOfMeasure { get; }

        SIState SIState { get; }

        double Quantity { get; }
    }
}

