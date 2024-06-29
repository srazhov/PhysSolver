using PhysHelper.SIObjects.Scalars;

namespace PhysHelper.SIObjects.Forces
{
    public class KineticFrictionForce : Force
    {
        public KineticFrictionForce(string anotherObjId, double k, double mass, double angle) : base(mass, Constants.Forces.g_Earth, angle)
        {
            Coefficient = new KineticFrictionCoefficient()
            {
                SIState = Enums.SIState.Known,
                Quantity = k
            };

            FrictionBetweenAnotherObjectId = anotherObjId;
            Quantity *= k;
        }

        public string FrictionBetweenAnotherObjectId { get; }

        public KineticFrictionCoefficient Coefficient { get; }

        public override VectorQuantity Direction
        {
            get
            {
                var dir = base.Direction;
                dir.Y = 0;

                return dir;
            }
        }
    }
}

