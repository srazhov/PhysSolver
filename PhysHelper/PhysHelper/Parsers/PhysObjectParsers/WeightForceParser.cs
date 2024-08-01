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
                obj.Forces.Add(new Force(obj.Mass, gAcc, 270));

                // Add Weight Forces for Objects that are on top
            }
        }
    }
}

