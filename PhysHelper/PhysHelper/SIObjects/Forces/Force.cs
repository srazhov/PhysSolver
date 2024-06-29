namespace PhysHelper.SIObjects.Forces
{
    public class Force : Vector
    {
        public override string UnitOfMeasure => Constants.Forces.Newton;

        public Force(double mass, double acceleration, double angle)
        {
            Quantity = mass * acceleration;
            Angle = angle;
        }

        public Force(double quantity, double angle)
        {
            Quantity = quantity;
            Angle = angle;
        }
    }
}

