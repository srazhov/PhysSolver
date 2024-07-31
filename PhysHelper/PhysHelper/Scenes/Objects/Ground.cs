using PhysHelper.SIObjects.Scalars;

namespace PhysHelper.Scenes.Objects
{
    public class Ground : PhysObject
    {
        public Ground() : base(new Mass() { Quantity = 0, SIState = Enums.SIState.Unimportant }, []) { }
    }
}
