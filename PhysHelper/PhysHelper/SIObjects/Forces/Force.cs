using PhysHelper.SIObjects.Kinematics;
using PhysHelper.SIObjects.Scalars;

namespace PhysHelper.SIObjects.Forces
{
    public class Force : Vector
    {
        public Force(Mass mass, Acceleration acceleration, double angle) : base(acceleration.Quantity * mass.Quantity, angle)
        {
            Acceleration = acceleration;
            Mass = mass;

            if (Acceleration.SIState != Enums.SIState.Known || mass.SIState != Enums.SIState.Known)
            {
                SIState = Enums.SIState.Unimportant;
                Quantity = 0;
            }
        }

        public Force(double quantity, double angle) : base(quantity, angle)
        {
            Acceleration = new Acceleration();
            Mass = new Mass();
        }

        public override string UnitOfMeasure => Constants.Forces.Newton;

        public Acceleration Acceleration { get; private set; }

        public Mass Mass { get; private set; }
    }
}

