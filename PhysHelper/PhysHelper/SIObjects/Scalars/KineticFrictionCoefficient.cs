namespace PhysHelper.SIObjects.Scalars
{
    public class KineticFrictionCoefficient(double k) : Scalar(k)
    {
        public override string UnitOfMeasure => Constants.Scalars.KineticFrictionCoefficient;
    }
}

