using PhysHelper.Parsers.PhysObjectParsers;
using PhysHelper.Scenes.Objects;
using PhysHelper.Scenes.SceneSettings;

namespace PhysHelper.Parsers
{
    public class PhysObjectsParser
    {
        public static List<IPhysObject> Parse(SceneSettings settings)
        {
            // ObjectsCreationParser must always be first
            IParserHandler<List<IPhysObject>, SceneSettings> parser = new ObjectsCreationParser();

            parser
                .SetNext(new WeightForceParser())
                .SetNext(new NormalForceParser()) // NormalForceParser must be after WeightForce
                .SetNext(new AdditionalForceParser())
                .SetNext(new ElasticForceParser())
                //.SetNext(new StaticForceParser())
                .SetNext(new KineticFrictionForceParser()) // KineticFrictionForceParser must be after NormalForce
                .SetNext(new NetForceParser()); // NetForceParser must be in the end

            var result = new List<IPhysObject>();
            parser.Parse(result, settings);

            return result;
        }
    }
}

