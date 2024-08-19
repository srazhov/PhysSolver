using PhysHelper.Parsers;
using PhysHelper.Scenes.Objects;

namespace PhysHelper.Scenes
{
    public class Scene(SceneSettings.SceneSettings settings) : IScene
    {
        private List<IPhysObject> Objects { get; } = PhysObjectsParser.Parse(settings);

        public IPhysObject[] GetAllObjects() => [.. Objects];
    }
}

