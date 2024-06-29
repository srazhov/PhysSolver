using PhysHelper.Enums;
using PhysHelper.Scenes;
using PhysHelper.Scenes.Implementation;

namespace PhysHelper.Factories
{
    public static class ReferenceSystemFactory
    {
        public static IReferenceSystem GetReferenceSystem(ReferenceSystemState state)
        {
            return state switch
            {
                ReferenceSystemState.Absolute => new ReferenceSystem(0),
                _ => throw new NotImplementedException(),
            };
        }
    }
}

