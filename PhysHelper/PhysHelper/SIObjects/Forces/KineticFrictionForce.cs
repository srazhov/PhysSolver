using PhysHelper.SIObjects.Scalars;

namespace PhysHelper.SIObjects.Forces
{
    public class KineticFrictionForce : Force
    {
        public KineticFrictionForce(double mu, double normalForce, double angle, Mass mass) : base(normalForce * mu, angle, Enums.ForceType.KineticFriction)
        {
            Coefficient = new KineticFrictionCoefficient(mu);
            Mass = mass;
        }

        public KineticFrictionCoefficient Coefficient { get; }
    }
}

