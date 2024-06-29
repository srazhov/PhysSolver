using PhysHelper.Enums;
using PhysHelper.SIObjects.Forces;

namespace PhysHelper.Scenes
{
	public interface IPhysObject
	{
		IReferenceSystem GetRefSystem();

		void ChangeRefSystem(ReferenceSystemState state);

		IEnumerable<Force> GetAllForces(); 
	}
}

