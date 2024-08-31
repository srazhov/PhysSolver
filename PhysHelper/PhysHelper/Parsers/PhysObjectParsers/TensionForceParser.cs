using PhysHelper.Enums;
using PhysHelper.Scenes.Objects;
using PhysHelper.Scenes.SceneSettings;
using PhysHelper.SIObjects.Forces;

namespace PhysHelper.Parsers.PhysObjectParsers;

public class TensionForceParser : BaseParserHandler<List<IPhysObject>, SceneSettings>
{
    protected override void Handle(List<IPhysObject> parsedObj, SceneSettings query)
    {
        if (query.Tensions == null)
        {
            return;
        }

        foreach (var obj in parsedObj)
        {
            var tensionSetting = query.Tensions.SingleOrDefault(x => x.TargetObj == obj.GetId());
            if (tensionSetting == null)
            {
                continue;
            }

            var secondObj = parsedObj.Single(x => x.GetId() == tensionSetting.SecondObj);
            double totalX = 0;
            if (!obj.Forces.Any(x => x.SIState != SIState.Known))
            {
                totalX = CalculateTensionForce(obj.Forces);
            }
            else if (!secondObj.Forces.Any(x => x.SIState != SIState.Known))
            {
                totalX = CalculateTensionForce(secondObj.Forces);
            }
            else
            {
                throw new NotImplementedException();
            }

            obj.Forces.Add(new TensionForce(totalX, tensionSetting.TargetObjAngle));
            secondObj.Forces.Add(new TensionForce(totalX, tensionSetting.SecondObjAngle));
        }
    }

    private static double CalculateTensionForce(List<Force> forces)
    {
        double totalX = 0;
        foreach (var force in forces)
        {
            totalX += force.Direction.X;
        }

        return totalX;
    }
}