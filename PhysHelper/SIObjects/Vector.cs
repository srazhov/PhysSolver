using PhysHelper.SIObjects.Enums;

namespace PhysHelper.SIObjects
{
    public abstract class Vector : ISIObject
    {
        public bool IsIt3D { get; }

        public VectorQuantity? CurrentPosition { get; set; }

        public required VectorQuantity Direction { get; set; }

        public double Magnitude
        {
            get
            {
                return IsIt3D ?
                    Math.Sqrt(Math.Pow(Direction.X, 2) + Math.Pow(Direction.Y, 2) + Math.Pow(Direction.Z, 2)) :
                    Math.Sqrt(Math.Pow(Direction.X, 2) + Math.Pow(Direction.Y, 2));
            }
        }

        public double Angle { get; set; }

        public abstract string UnitOfMeasure { get; }

        public SIState SIState { get; }

        public class VectorQuantity
        {
            public double X { get; set; }

            public double Y { get; set; }

            public double Z { get; set; }
        }
    }
}

