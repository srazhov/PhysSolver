using PhysHelper.Scenes.Objects;

namespace PhysHelper.Scenes
{
	public interface ISceneMaker
	{
		IEnumerable<IPhysObject> GetObjects();
	}
}

