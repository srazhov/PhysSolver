using PhysHelper.Scenes.Objects;

namespace PhysHelper.Scenes
{
    public interface IScene
    {
        IPhysObject[] GetAllObjects();
    }
}

