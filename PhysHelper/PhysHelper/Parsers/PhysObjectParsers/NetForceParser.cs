using System.Numerics;
using PhysHelper.Enums;
using PhysHelper.Scenes.Objects;
using PhysHelper.Scenes.SceneSettings;
using PhysHelper.SIObjects;
using PhysHelper.SIObjects.Forces;

namespace PhysHelper.Parsers.PhysObjectParsers;

public class NetForceParser : BaseParserHandler<List<IPhysObject>, SceneSettings>
{
    protected override void Handle(List<IPhysObject> parsedObj, SceneSettings query)
    {
        foreach (var obj in parsedObj)
        {
            if (obj.Forces.Count > 0 && !obj.Forces.Any(x => x.ForceType == ForceType.Net) && obj.Forces.All(x => x.SIState == SIState.Known))
            {
                var netForce = GetNetForce(obj.Forces);
                obj.Forces.Add(netForce);
            }
        }
    }

    private static Force GetNetForce(List<Force> forces)
    {
        double netForceX = 0;
        double netForceY = 0;
        for (int i = 0; i < forces.Count; i++)
        {
            netForceX += forces[i].Direction.X;
            netForceY += forces[i].Direction.Y;
        }

        var vector = new VectorQuantity(netForceX, netForceY, true);

        return new Force(vector.Quantity, vector.Angle, ForceType.Net);
    }
}