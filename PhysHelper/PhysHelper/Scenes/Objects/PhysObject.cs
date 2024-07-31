using PhysHelper.SIObjects.Forces;
using PhysHelper.SIObjects.Scalars;

namespace PhysHelper.Scenes.Objects
{
    public abstract class PhysObject(Mass mass, List<Force> forces) : IPhysObject
    {
        private string? Id = null;

        public Mass Mass { get; private set; } = mass;

        public List<Force> Forces { get; private set; } = forces;

        public string GetId()
        {
            return Id ??= Guid.NewGuid().ToString();
        }
    }
}

