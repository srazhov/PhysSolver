using PhysHelper.Scenes.Objects;
using PhysHelper.SIObjects.Forces;

namespace PhysHelper.Scenes.ObjectRelations
{
    public class ObjectRelation
    {
        public IPhysObject? BottomObject { get; private set; } = null;

        public Force? MovingForce { get; private set; }

        public double KineticCoefficient { get; private set; } = 0;

        public ObjectRelation IsOnTopOf(IPhysObject secondObj)
        {
            BottomObject = secondObj;

            return this;
        }

        public ObjectRelation IsOnTheGround()
        {
            if (BottomObject != null)
            {
                throw new ArgumentException("Object is already lies on another object");
            }

            BottomObject = new Ground();
            return this;
        }

        public ObjectRelation HasConstantForce(Force force)
        {
            MovingForce = force;
            return this;
        }

        public ObjectRelation HaveKineticFriction(double k)
        {
            if (BottomObject == null)
            {
                throw new ArgumentException("Cannot assign constant K if there are no bodies to interact with");
            }

            KineticCoefficient = k;
            return this;
        }
    }
}

