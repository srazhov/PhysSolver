using PhysHelper.SIObjects.Kinematics;
using PhysHelper.SIObjects.Scalars;

namespace PhysHelper.SIObjects.Forces
{
    public class KineticFrictionForce : Force
    {
        public KineticFrictionForce(double mu, Mass mass, Acceleration acceleration, double angle) : base(
            mass,
            acceleration,
            angle,
            Enums.ForceType.KineticFriction)
        {
            Coefficient = new KineticFrictionCoefficient(mu);
            Quantity *= Coefficient.Quantity;
        }

        public KineticFrictionCoefficient Coefficient { get; }
    }
}

