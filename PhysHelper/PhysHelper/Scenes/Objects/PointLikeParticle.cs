using PhysHelper.SIObjects.Forces;
using PhysHelper.SIObjects.Scalars;

namespace PhysHelper.Scenes.Objects
{
    public class PointLikeParticle(Mass mass, List<Force> forces, string? objName = null) : PhysObject(mass, forces, objName)
    {

    }
}

