using PhysHelper.Enums;
using PhysHelper.Scenes.Objects;
using PhysHelper.Scenes.SceneSettings;
using PhysHelper.SIObjects;
using PhysHelper.SIObjects.Forces;

namespace PhysHelper.Parsers.PhysObjectParsers
{
    public class KineticFrictionForceParser : BaseParserHandler<List<IPhysObject>, SceneSettings>
    {
        protected override void Handle(List<IPhysObject> parsedObj, SceneSettings query)
        {
            if (query.ObjectsFriction == null)
            {
                return;
            }

            foreach (var obj in parsedObj.Where(x => x.GetId() != Constants.GroundId))
            {
                var normalForces = obj.Forces.Where(x => x.ForceType == ForceType.Normal);
                if (!normalForces.Any())
                {
                    continue;
                }

                var curId = obj.GetId();
                var objOrder = query.ObjectsPlacementOrder.Single(x => x.Contains(curId));
                foreach (var kF in query.ObjectsFriction.Where(x => x.FirstObj == curId || x.SecondObj == curId))
                {
                    var bottomObjId = objOrder.IndexOf(kF.FirstObj) < objOrder.IndexOf(kF.SecondObj) ? kF.FirstObj : kF.SecondObj;
                    var startFromId = bottomObjId == curId ? curId : bottomObjId;

                    var forcesToAdd = new List<KineticFrictionForce>();
                    for (int i = objOrder.IndexOf(startFromId) + 1; i < objOrder.Count; i++)
                    {
                        var topObjMass = query.Objects.First(x => x.Name == objOrder[i]).Mass.Quantity;
                        var f = normalForces.First(x => x.Mass.Quantity == topObjMass);
                        forcesToAdd.Add(new KineticFrictionForce(kF.K, f.Mass, f.Acceleration, kF.Angle));
                    }

                    obj.Forces.AddRange(forcesToAdd);
                }
            }
        }
    }
}
