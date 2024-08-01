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
                if (obj is Ground)
                {
                    continue;
                }

                obj.Forces.Add(new Force(obj.Mass, gAcc, 270));

                var curObjId = obj.GetId();
                var placementOrder = query.ObjectsPlacementOrder.Single(x => x.Contains(curObjId));
                for (var i = placementOrder.FindIndex(x => x == curObjId) + 1; i < placementOrder.Count; i++)
                {
                    var anotherObj = parsedObj.Single(x => x.GetId() == placementOrder[i]);
                    obj.Forces.Add(new Force(anotherObj.Mass, gAcc, 270));
                }
            }
        }
    }
}

