using PhysHelper.Enums;
using PhysHelper.Scenes.Objects;
using PhysHelper.Scenes.SceneSettings;
using PhysHelper.SIObjects;
using PhysHelper.SIObjects.Forces;
using PhysHelper.SIObjects.Kinematics;

namespace PhysHelper.Parsers.PhysObjectParsers
{
    public class WeightForceParser : BaseParserHandler<List<IPhysObject>, SceneSettings>
    {
        protected override void Handle(List<IPhysObject> parsedObj, SceneSettings query)
        {
            var g = query.Global?.G ?? Constants.Forces.g_Earth;
            var gAcc = new Acceleration(g, 270);

            foreach (var obj in parsedObj)
            {
                obj.Forces.Add(new Force(obj.Mass, gAcc, 270, ForceType.Weight));
            }

            foreach (var obj in parsedObj)
            {
                var curObjId = obj.GetId();
                if (curObjId == Constants.GroundId)
                {
                    continue;
                }

                var placementOrder = query.ObjectsPlacementOrder.Single(x => x.Contains(curObjId));
                var forcesToAdd = new List<Force>();
                for (var i = placementOrder.FindIndex(x => x == curObjId) + 1; i < placementOrder.Count; i++)
                {
                    var anotherObjWeightForce = parsedObj.Single(x => x.GetId() == placementOrder[i])
                        .Forces.Single(x => x.ForceType == ForceType.Weight);
                    forcesToAdd.Add(anotherObjWeightForce);
                }

                obj.Forces.AddRange(forcesToAdd);
            }
        }
    }
}

