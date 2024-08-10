using PhysHelper.SIObjects.Kinematics;
using PhysHelper.SIObjects.Scalars;

namespace PhysHelper.SIObjects.Forces
{
    public class KineticFrictionForce : Force
    {
        public KineticFrictionForce(double k, Mass mass, Acceleration acceleration, double angle) : base(
            mass,
            acceleration,
            angle,
            Enums.ForceType.KineticFriction)
        {
            Coefficient = new KineticFrictionCoefficient(k);
            Quantity *= Coefficient.Quantity;
        }

        public KineticFrictionCoefficient Coefficient { get; }
    }
}

