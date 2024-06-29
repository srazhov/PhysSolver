namespace PhysHelper.Scenes.Implementation
{
    public class ReferenceSystem : IReferenceSystem
    {
        private double Angle { get; set; }

        public ReferenceSystem(double angle)
        {
            Angle = angle;
        }

        public double GetAngle()
        {
            return Angle;
        }
    }
}

