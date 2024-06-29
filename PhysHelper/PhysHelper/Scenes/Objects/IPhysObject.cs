using PhysHelper.Enums;
using PhysHelper.SIObjects.Forces;

namespace PhysHelper.Scenes.Objects
{
    public interface IPhysObject
    {
        string GetId();

        double Mass { get; }

        IReferenceSystem GetRefSystem();

        void ChangeRefSystem(ReferenceSystemState state);

        List<Force> GetAllForces();

        void AddForce(Force force);
    }
}

