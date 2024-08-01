using PhysHelper.Scenes.Objects;

namespace PhysHelper.Scenes
{
    public class Scene : IScene
    {
        private List<IPhysObject> Objects { get; }

        public Scene()
        {
            Objects = [
                new Ground()
            ];
        }

        public IPhysObject[] GetAllObjects() => [.. Objects];

        public void AddNewObject(IPhysObject obj)
        {
            var objId = obj.GetId();
            if (!Objects.Any(x => x.GetId() == objId))
            {
                Objects.Add(obj);
            }
        }
    }
}

