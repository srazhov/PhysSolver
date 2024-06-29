using PhysHelper.Enums;

namespace PhysHelper.Scenes.Implementation
{
    public class ReferenceSystem : IReferenceSystem
    {
        private double Angle { get; set; }

        private ReferenceSystemState RefState { get; set; }

        public ReferenceSystem(ReferenceSystemState state, double angle)
        {
            Angle = angle;
            RefState = state;
        }

        public double GetAngle()
        {
            return Angle;
        }

        public ReferenceSystemState GetSystemState()
        {
            return RefState;
        }
    }
}

