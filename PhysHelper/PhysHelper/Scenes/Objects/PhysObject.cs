using PhysHelper.SIObjects.Forces;
using PhysHelper.SIObjects.Scalars;

namespace PhysHelper.Scenes.Objects
{
    public abstract class PhysObject(Mass mass, List<Force> forces, string? id = null) : IPhysObject
    {
        private string? Id = id;

        public virtual Mass Mass { get; private set; } = mass;

        public virtual List<Force> Forces { get; private set; } = forces;

        public string GetId()
        {
            return Id ??= Guid.NewGuid().ToString();
        }
    }
}

