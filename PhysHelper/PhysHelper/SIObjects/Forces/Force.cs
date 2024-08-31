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

            if (forceType == ForceType.Net)
            {
                // mg + N + Fk = ma
                // Net Force must have opposite sign
                Quantity *= -1;
            }
        }

        public Force(double quantity, double angle, ForceType forceType) : base(quantity, angle)
        {
            Acceleration = new Acceleration();
            Mass = new Mass();
            ForceType = forceType;
        }

        public override string UnitOfMeasure => Constants.Forces.Newton;

        public Mass Mass { get; set; }

        public Acceleration Acceleration { get; set; }

        public ForceType ForceType { get; private set; }

        public override string ToString()
        {
            return $"{ForceType}: {Quantity}N at {Angle}deg";
        }
    }
}

