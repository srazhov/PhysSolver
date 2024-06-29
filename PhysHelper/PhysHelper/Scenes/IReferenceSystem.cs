using PhysHelper.Enums;

namespace PhysHelper.Scenes
{
    public interface IReferenceSystem
    {
        ReferenceSystemState GetSystemState();

        double GetAngle();
    }
}

