using System.Numerics;
using PhysHelper.Enums;
using PhysHelper.Scenes.Objects;
using PhysHelper.Scenes.SceneSettings;
using PhysHelper.SIObjects.Forces;
using static PhysHelper.SIObjects.Vector;

namespace PhysHelper.Parsers.PhysObjectParsers;

public class NetForceParser : BaseParserHandler<List<IPhysObject>, SceneSettings>
{
    protected override void Handle(List<IPhysObject> parsedObj, SceneSettings query)
    {
        foreach (var obj in parsedObj)
        {
            if (obj.Forces.Count > 0 && obj.Forces.All(x => x.SIState == SIState.Known))
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

        var magnitude = new VectorQuantity(netForceX, netForceY).Magnitude;
        var dotProduct = Vector2.Dot(new Vector2((float)netForceX, (float)netForceY), new Vector2(1, 0));
        var angle = magnitude == 0 ? 0 : Math.Round(Math.Acos(dotProduct / magnitude) * (180 / Math.PI), 2);

        return new Force(magnitude, angle, ForceType.Net);
    }
}