using PhysHelper.SIObjects.Kinematics;
using PhysHelper.SIObjects.Scalars;

namespace PhysHelper.SIObjects.Forces
{
    public class Force : Vector
    {
        public Force(Acceleration acceleration, Mass mass, double angle) : base(acceleration.Quantity * mass.Quantity, angle)
        {
            Acceleration = acceleration;
            Mass = mass;

            // TODO
            // To change behavior of assigning "Quantity" value 
            // in case if Acceleration or Mass are not defined
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

