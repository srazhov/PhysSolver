using PhysHelper.Enums;
using PhysHelper.SIObjects.Scalars;

namespace PhysHelper.SIObjects.Forces
{
    public class KineticFrictionForce : Force
    {
        public KineticFrictionForce(double mu, Force normalForce, double angle) :
            base(normalForce.Mass, normalForce.Acceleration, angle, ForceType.KineticFriction)
        {
            if (normalForce.ForceType != ForceType.Normal)
            {
                throw new ArgumentException("Provided force is not a Normal Force", nameof(normalForce));
            }

            Coefficient = new KineticFrictionCoefficient(mu);
            Quantity = mu * normalForce.Quantity;
        }

        public KineticFrictionCoefficient Coefficient { get; }
    }
}

