using PhysHelper.Scenes.Objects;

namespace PhysHelper.Scenes
{
    public interface IScene
    {
        void AddNewObject(IPhysObject obj, ObjectRelations.ObjectRelation relations);

        IEnumerable<IPhysObject> GetAllObjects();

        void FindXWhenY();
    }
}

