using PhysHelper.Enums;
using PhysHelper.Helpers;
using PhysHelper.Scenes.Objects;
using PhysHelper.Scenes.SceneSettings;
using PhysHelper.SIObjects.Forces;

namespace PhysHelper.Parsers.PhysObjectParsers
{
    public class NormalForceParser : BaseParserHandler<List<IPhysObject>, SceneSettings>
    {
        protected override void Handle(List<IPhysObject> parsedObj, SceneSettings query)
        {
            foreach (var obj in parsedObj)
            {
                if (query.ObjectsPlacementOrder.Any(x => x.FindIndex(y => y == obj.GetId()) == 0))
                {
                    continue;
                }

                var angle = query.Objects.Single(x => x.Name == obj.GetId()).Angle + 90;
                var forcesToAdd = new List<Force>();
                foreach (var force in obj.Forces.Where(x => x.ForceType == ForceType.Weight))
                {
                    var normalForceQuantity = Math.Round(force.Quantity * Math.Sin(HelperClass.GetAngleInRadians(angle)), 5);
                    forcesToAdd.Add(new Force(normalForceQuantity, angle, ForceType.Normal)
                    {
                        Mass = force.Mass,
                        Acceleration = force.Acceleration
                    });
                }

                obj.Forces.AddRange(forcesToAdd);
            }
        }
    }
}
