namespace PhysHelper.SIObjects.Scalars;

public class SpringExtension : Scalar
{
    public override string UnitOfMeasure => Constants.Scalars.SpringExtension;

    public SpringExtension(double quantity) : base(quantity) { }
}