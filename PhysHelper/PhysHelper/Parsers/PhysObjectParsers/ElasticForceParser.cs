using PhysHelper.Scenes.Objects;
using PhysHelper.Scenes.SceneSettings;
using PhysHelper.SIObjects;
using PhysHelper.SIObjects.Forces;

namespace PhysHelper.Parsers.PhysObjectParsers;

public class ElasticForceParser : BaseParserHandler<List<IPhysObject>, SceneSettings>
{
    protected override void Handle(List<IPhysObject> parsedObj, SceneSettings query)
    {
        foreach (var obj in parsedObj)
        {
            var curElasticForce = query.Objects.SingleOrDefault(x => x.Name == obj.GetId())?.ElasticForce;
            if (curElasticForce == null)
            {
                continue;
            }

            obj.Forces.Add(new ElasticForce(curElasticForce.K, curElasticForce.X, curElasticForce.RestingLength, curElasticForce.Angle));
        }
    }
}
