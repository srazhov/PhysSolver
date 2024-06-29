using PhysHelper.Enums;

namespace PhysHelper.Scenes.Implementation
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

