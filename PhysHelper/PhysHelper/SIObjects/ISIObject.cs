using PhysHelper.SIObjects.Enums;

namespace PhysHelper.SIObjects
{
	public interface ISIObject
	{
		string UnitOfMeasure { get; }

		SIState SIState { get; }
	}
}

