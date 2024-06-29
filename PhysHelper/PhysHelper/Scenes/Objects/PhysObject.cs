using PhysHelper.Enums;
using PhysHelper.Factories;
using PhysHelper.SIObjects.Forces;

namespace PhysHelper.Scenes.Objects
{
    public abstract class PhysObject : IPhysObject
    {
        public double Mass { get; private set; }

        private IReferenceSystem RefSystem { get; set; }

        public PhysObject(double mass)
        {
            Mass = mass;

            RefSystem = ReferenceSystemFactory.GetReferenceSystem(ReferenceSystemState.Absolute);
        }

        public List<Force> GetAllForces()
        {
            var forces = new List<Force>
            {
                ForceFactory.GetWeightForce(Mass),
                ForceFactory.GetNormalForce(Mass)
            };

            return forces;
        }

        public IReferenceSystem GetRefSystem()
        {
            return RefSystem;
        }

        public void ChangeRefSystem(ReferenceSystemState state)
        {
            RefSystem = ReferenceSystemFactory.GetReferenceSystem(state);
        }
    }
}

