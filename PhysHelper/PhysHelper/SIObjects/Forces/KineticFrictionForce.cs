using PhysHelper.SIObjects.Kinematics;
using PhysHelper.SIObjects.Scalars;

namespace PhysHelper.SIObjects.Forces
{
    public class KineticFrictionForce : Force
    {
        public KineticFrictionForce(double k, double mass, double angle) : base(
            new Acceleration(Constants.Forces.g_Earth, 270),
            new Mass(mass),
            angle)
        {
            Coefficient = new KineticFrictionCoefficient(k);
            Quantity *= Coefficient.Quantity;
        }

        public KineticFrictionCoefficient Coefficient { get; }

        public override VectorQuantity Direction
        {
            get
            {
                var dir = base.Direction;
                dir.Y = 0;

                return dir;
            }
        }
    }
}

