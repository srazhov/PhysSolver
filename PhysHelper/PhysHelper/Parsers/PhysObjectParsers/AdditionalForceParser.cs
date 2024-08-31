using PhysHelper.Enums;
using PhysHelper.Scenes.Objects;
using PhysHelper.Scenes.SceneSettings;
using PhysHelper.SIObjects.Forces;
using PhysHelper.SIObjects.Kinematics;

namespace PhysHelper.Parsers.PhysObjectParsers;

public class AdditionalForceParser : BaseParserHandler<List<IPhysObject>, SceneSettings>
{
    protected override void Handle(List<IPhysObject> parsedObj, SceneSettings query)
    {
        foreach (var obj in parsedObj)
        {
            if (obj is Ground)
            {
                continue;
            }

            var thisObjSetting = query.Objects.Single(x => x.Name == obj.GetId());
            for (int i = 0; i < thisObjSetting.Forces?.Count; i++)
            {
                var forceType = thisObjSetting.Forces[i].IsNetForce ? ForceType.Net : ForceType.Additional;
                var angle = thisObjSetting.Forces[i].Angle;
                var siState = thisObjSetting.Forces[i].SiState;

                if (thisObjSetting.Forces[i].Acceleration != null)
                {
                    var acc = new Acceleration(thisObjSetting.Forces[i].Acceleration.Quantity, angle)
                    {
                        SIState = thisObjSetting.Forces[i].Acceleration.SiState
                    };

                    obj.Forces.Add(new Force(obj.Mass, acc, angle, forceType) { SIState = siState });
                }
                else
                {
                    obj.Forces.Add(new Force(thisObjSetting.Forces[i].Quantity, angle, forceType)
                    {
                        Mass = obj.Mass,
                        SIState = siState
                    });
                }
            }
        }
    }
}