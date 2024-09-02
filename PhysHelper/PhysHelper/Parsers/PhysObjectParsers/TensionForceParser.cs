using PhysHelper.Enums;
using PhysHelper.Helpers;
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
            double resultTensionForce = 0;
            if (!obj.Forces.Any(x => x.SIState != SIState.Known))
            {
                resultTensionForce = CalculateTensionForce(obj.Forces, tensionSetting.TargetObjAngle);
            }
            else if (!secondObj.Forces.Any(x => x.SIState != SIState.Known))
            {
                resultTensionForce = CalculateTensionForce(secondObj.Forces, tensionSetting.SecondObjAngle);
            }
            else
            {
                // TODO
                // If both objects' forces are not fully known
                throw new NotImplementedException();
            }

            obj.Forces.Add(new TensionForce(resultTensionForce, tensionSetting.TargetObjAngle));
            secondObj.Forces.Add(new TensionForce(resultTensionForce, tensionSetting.SecondObjAngle));
        }
    }

    private static double CalculateTensionForce(List<Force> forces, double angle)
    {
        var total = SumAllForces(forces);
        var cos = Math.Round(Math.Cos(HelperClass.GetAngleInRadians(angle)), 8);
        if (cos != 0)
        {
            return Math.Abs(Math.Round(total.Item1 / cos, 5));
        }
        else
        {
            var sin = Math.Round(Math.Sin(HelperClass.GetAngleInRadians(angle)), 8);
            return Math.Abs(Math.Round(total.Item2 / sin, 5));
        }
    }

    private static (double, double) SumAllForces(List<Force> forces)
    {
        double totalX = 0;
        double totalY = 0;
        foreach (var force in forces)
        {
            var multiplier = force.ForceType != ForceType.Net ? -1 : 1;
            totalX += force.Direction.X * multiplier;
            totalY += force.Direction.Y;
        }

        return (totalX, totalY);
    }
}