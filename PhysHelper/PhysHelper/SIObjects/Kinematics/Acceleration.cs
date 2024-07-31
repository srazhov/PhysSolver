namespace PhysHelper.SIObjects.Kinematics
{
    public class Acceleration(double quantity, double angle) : Vector(quantity, angle)
    {
        public override string UnitOfMeasure => Constants.Kinematics.Acceleration;
    }
}

