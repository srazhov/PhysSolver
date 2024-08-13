namespace PhysHelper.SIObjects.Scalars;

public class SpringRestingLength : Scalar
{
    public override string UnitOfMeasure => Constants.Scalars.SpringExtension;

    public SpringRestingLength(double l) : base(l) { }
}