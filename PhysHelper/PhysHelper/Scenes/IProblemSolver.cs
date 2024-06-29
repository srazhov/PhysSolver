using PhysHelper.SIObjects;

namespace PhysHelper.Scenes
{
	public interface IProblemSolver
	{
		ISIObject SolveFor(ISIObject x);
	}
}
