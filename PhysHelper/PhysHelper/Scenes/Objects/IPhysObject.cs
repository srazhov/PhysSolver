using PhysHelper.Enums;
using PhysHelper.SIObjects.Forces;

namespace PhysHelper.Scenes.Objects
{
	public interface IPhysObject
	{
		double Mass { get; }

		IReferenceSystem GetRefSystem();

		void ChangeRefSystem(ReferenceSystemState state);

		List<Force> GetAllForces();
	}
}

