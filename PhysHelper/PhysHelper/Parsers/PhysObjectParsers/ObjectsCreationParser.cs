using PhysHelper.Scenes.Objects;
using PhysHelper.Scenes.SceneSettings;
using PhysHelper.SIObjects.Scalars;

namespace PhysHelper.Parsers.PhysObjectParsers;

public class ObjectsCreationParser : BaseParserHandler<List<IPhysObject>, SceneSettings>
{
    protected override void Handle(List<IPhysObject> parsedObj, SceneSettings query)
    {
        if (query.Ground?.Exists == true)
        {
            parsedObj.Add(new Ground());
        }

        foreach (var obj in query.Objects)
        {
            var mass = new Mass(obj.Mass.Quantity) { SIState = obj.Mass.SiState };
            parsedObj.Add(new PointLikeParticle(mass, [], obj.Name));
        }
    }
}