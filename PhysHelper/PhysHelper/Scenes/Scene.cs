using PhysHelper.Factories;
using PhysHelper.Scenes.ObjectRelations;
using PhysHelper.Scenes.Objects;

namespace PhysHelper.Scenes
{
    public class Scene : IScene
    {
        private List<IPhysObject>? Objects;

        public IEnumerable<IPhysObject> GetAllObjects()
        {
            return Objects ??= new List<IPhysObject>()
            {
                new Ground()
            };
        }

        public void FindXWhenY()
        {
            throw new NotImplementedException();
        }

        public void AddNewObject(IPhysObject obj, ObjectRelation relations)
        {
            var objects = GetAllObjects();
            var objId = obj.GetId();
            if (objects.Any(x => x.GetId() == objId))
            {
                throw new ArgumentException("Object is already present in the scene");
            }

            if (relations.MovingForce != null)
            {
                obj.AddForce(relations.MovingForce);
            }

            if (relations.KineticCoefficient > 0 && relations.BottomObject != null)
            {
                ForceFactory.CreateKineticFrictionForceBetweenObjects(obj, relations.BottomObject,
                    relations.KineticCoefficient, obj.Mass, 0);
            }
        }
    }
}

