using PhysHelper.Enums;
using PhysHelper.Factories;
using PhysHelper.SIObjects.Forces;

namespace PhysHelper.Scenes.Objects
{
    public abstract class PhysObject : IPhysObject
    {
        private IReferenceSystem RefSystem;

        private string? Id = null;

        private List<Force>? Forces = null;

        public double Mass { get; private set; }

        public PhysObject(double mass)
        {
            Mass = mass;

            RefSystem = ReferenceSystemFactory.GetReferenceSystem(ReferenceSystemState.Absolute);
        }

        public string GetId()
        {
            return Id ??= Guid.NewGuid().ToString();
        }

        public List<Force> GetAllForces()
        {
            return Forces ??= new List<Force>
            {
                ForceFactory.GetWeightForce(Mass),
                ForceFactory.GetNormalForce(Mass)
            };
        }

        public IReferenceSystem GetRefSystem()
        {
            return RefSystem;
        }

        public void ChangeRefSystem(ReferenceSystemState state)
        {
            RefSystem = ReferenceSystemFactory.GetReferenceSystem(state);
        }

        public void AddForce(Force force)
        {
            var forces = GetAllForces();
            if (force is KineticFrictionForce kF)
            {
                if (forces.OfType<KineticFrictionForce>().Any(x => x.FrictionBetweenAnotherObjectId == kF.FrictionBetweenAnotherObjectId))
                {
                    throw new ArgumentOutOfRangeException(nameof(force), "Kinetic friction force is already exists in this object");
                }
            }

            forces.Add(force);
        }
    }
}

