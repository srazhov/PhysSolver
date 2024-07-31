using PhysHelper.SIObjects.Forces;
using PhysHelper.SIObjects.Scalars;

namespace PhysHelper.Scenes.Objects
{
    public interface IPhysObject
    {
        string GetId();

        Mass Mass { get; }

        List<Force> Forces { get; }
    }
}

