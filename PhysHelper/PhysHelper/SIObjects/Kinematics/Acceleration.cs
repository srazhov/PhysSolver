namespace PhysHelper.SIObjects.Kinematics
{
    public class Acceleration : Vector
    {
        public override string UnitOfMeasure => Constants.Kinematics.Acceleration;

        public Acceleration() : base() { }

        public Acceleration(double quantity, double angle) : base(quantity, angle) { }
    }
}

