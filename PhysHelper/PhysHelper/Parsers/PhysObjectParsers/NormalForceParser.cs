using PhysHelper.Enums;
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
                if (obj is Ground ||
                    query.ObjectsPlacementOrder.Any(x => x.FindIndex(y => y == obj.GetId()) == 0))
                {
                    continue;
                }

                var angle = query.Objects.Single(x => x.Name == obj.GetId()).Angle + 90;
                var forcesToAdd = new List<Force>();
                foreach (var force in obj.Forces.Where(x => x.ForceType == ForceType.Weight))
                {
                    forcesToAdd.Add(new Force(force.Mass, force.Acceleration, angle, ForceType.Normal));
                }

                obj.Forces.AddRange(forcesToAdd);
            }
        }
    }
}

