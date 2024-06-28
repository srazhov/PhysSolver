using PhysHelper.SIObjects.Kinematics;

namespace PhysHelper.SIObjects.Forces
{
    public class Force : Vector
    {
        public override string UnitOfMeasure => Constants.Forces.Newton;

        public required Mass Mass { get; set; }

        public required Acceleration Acceleration { get; set; }
    }
}

