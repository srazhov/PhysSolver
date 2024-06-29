using PhysHelper.Enums;
using PhysHelper.SIObjects.Forces;

namespace PhysHelper.Scenes.Implementation
{
    public class PhysObject : IPhysObject
    {
        public double Mass { get; private set; }

        public PhysObject(double mass)
        {
            Mass = mass;
        }

        public IEnumerable<Force> GetAllForces()
        {
            var forces = new List<Force>
            {
                ForceFactory.GetWeightForce(Mass)
            };

            return forces;
        }

        public IReferenceSystem GetRefSystem()
        {
            throw new NotImplementedException();
        }

        public void ChangeRefSystem(ReferenceSystemState state)
        {
            throw new NotImplementedException();
        }
    }
}

