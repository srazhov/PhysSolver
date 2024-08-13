namespace PhysHelper.SIObjects.Scalars;

public class SpringConstant : Scalar
{
    public override string UnitOfMeasure => Constants.Scalars.SpringConstant;

    public SpringConstant(double quantity) : base(quantity) { }
}