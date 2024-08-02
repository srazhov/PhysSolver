using PhysHelper.Enums;
using PhysHelper.SIObjects.Kinematics;
using PhysHelper.SIObjects.Scalars;

namespace PhysHelper.SIObjects.Forces
{
    public class Force : Vector
    {
        public Force(Mass mass, Acceleration acceleration, double angle, ForceType forceType) : base(acceleration.Quantity * mass.Quantity, angle)
        {
            Acceleration = acceleration;
            Mass = mass;
            ForceType = forceType;

            if (Acceleration.SIState != SIState.Known || mass.SIState != SIState.Known)
            {
                SIState = SIState.Unimportant;
                Quantity = 0;
            }
        }

        public Force(double quantity, double angle, ForceType forceType) : base(quantity, angle)
        {
            Acceleration = new Acceleration();
            Mass = new Mass();
            ForceType = forceType;
        }

        public override string UnitOfMeasure => Constants.Forces.Newton;

        public Acceleration Acceleration { get; private set; }

        public Mass Mass { get; private set; }

        public ForceType ForceType { get; private set; }
    }
}

