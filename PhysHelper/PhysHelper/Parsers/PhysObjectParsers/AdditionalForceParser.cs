using PhysHelper.Enums;
using PhysHelper.Scenes.Objects;
using PhysHelper.Scenes.SceneSettings;
using PhysHelper.SIObjects.Forces;

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
                obj.Forces.Add(new Force(thisObjSetting.Forces[i].Quantity, thisObjSetting.Forces[i].Angle, ForceType.Additional));
            }
        }
    }
}