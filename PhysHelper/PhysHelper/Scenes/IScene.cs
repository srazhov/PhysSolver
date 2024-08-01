using PhysHelper.Scenes.Objects;

namespace PhysHelper.Scenes
{
    public interface IScene
    {
        void AddNewObject(IPhysObject obj);

        IPhysObject[] GetAllObjects();
    }
}

