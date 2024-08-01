namespace PhysHelper.SIObjects.Scalars
{
    public class Mass : Scalar
    {
        public override string UnitOfMeasure => Constants.Scalars.Mass;

        public Mass(double quantity) : base(quantity)
        {
            Quantity = quantity;
        }

        public Mass() : base() { }
    }
}

