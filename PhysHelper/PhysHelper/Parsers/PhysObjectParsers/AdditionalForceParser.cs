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
                var angle = thisObjSetting.Forces[i].Angle;
                var accSetting = thisObjSetting.Forces[i].Acceleration;

                Acceleration acc;
                if (accSetting != null)
                {
                    acc = new Acceleration(accSetting.Quantity, angle)
                    {
                        SIState = accSetting.SiState
                    };
                }
                else
                {
                    var accQuantity = thisObjSetting.Forces[i].Quantity / obj.Mass.Quantity;
                    acc = new Acceleration(accQuantity, angle);
                }

                var forceType = thisObjSetting.Forces[i].IsNetForce ? ForceType.Net : ForceType.Additional;
                obj.Forces.Add(new Force(obj.Mass, acc, angle, forceType)
                {
                    SIState = thisObjSetting.Forces[i].SiState
                });
            }
        }
    }
}
