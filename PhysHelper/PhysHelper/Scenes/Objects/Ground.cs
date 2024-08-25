using PhysHelper.SIObjects;
using PhysHelper.SIObjects.Forces;
using PhysHelper.SIObjects.Scalars;

namespace PhysHelper.Scenes.Objects
{
    public class Ground : PhysObject
    {
        public Ground() : base(new Mass(), [], Constants.GroundId) { }

        public override List<Force> Forces => [];
    }
}
