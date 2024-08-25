using PhysHelper.Enums;
using PhysHelper.Scenes.Objects;
using PhysHelper.Scenes.SceneSettings;
using PhysHelper.SIObjects.Forces;
using PhysHelper.SIObjects.Scalars;

namespace PhysHelper.Parsers.PhysObjectParsers;

public class StaticForceParser : BaseParserHandler<List<IPhysObject>, SceneSettings>
{
    // TODO
    protected override void Handle(List<IPhysObject> parsedObj, SceneSettings query)
    {
        if (query.ObjectsFriction == null)
        {
            return;
        }

        foreach (var obj in parsedObj)
        {
            var curObjId = obj.GetId();
            if (curObjId == "ground")
            {
                continue;
            }

            var frictionSettings = query.ObjectsFriction.Where(x => x.TargetObj == curObjId).SingleOrDefault(x => !x.ObjectIsMoving);
            if (frictionSettings == null)
            {
                continue;
            }

            var forcesToAdd = new List<Force>();
            foreach (var force in obj.Forces.Where(x => x.ForceType == ForceType.Normal))
            {
                var mu = new StaticFrictionCoefficient()
                {
                    Quantity = frictionSettings.Mu,
                    SIState = frictionSettings.Mu > 0 ? SIState.Known : SIState.Unimportant
                };

                forcesToAdd.Add(new StaticForce(force, mu, frictionSettings.Angle));
            }

            obj.Forces.AddRange(forcesToAdd);
        }
    }
}
