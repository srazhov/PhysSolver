using PhysHelper.Parsers.PhysObjectParsers;
using PhysHelper.Scenes.SceneSettings;
using PhysHelper.SIObjects.Forces;

namespace PhysHelper.Tests.Parsers.PhysObjectParsers;

public class ElasticForceParserTests
{
    [Test]
    public void ElasticForceSettingIsPassed_MustAddElasticForceToAnObject()
    {
        // Arrange
        var parser = new ElasticForceParser();
        var results = PhysObjectHelpers.GetDefaultObjects(g: null, angle: 0, addM2: true, addGround: true, addNormalForce: false);
        var query = PhysObjectHelpers.GetDefaultSceneSettings(g: null, angle: 0, addM2: true, addGround: true);

        query.Objects.Single(x => x.Name == "m2").ElasticForce = null;
        query.Objects.Single(x => x.Name == "m1").ElasticForce = new ElasticForceSettings()
        {
            K = 200,
            X = 0.5,
            RestingLength = 1,
            Angle = 30
        };

        // Act
        parser.Parse(results, query);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(results, Has.Exactly(3).Items);

            var m1Obj = results.Single(x => x.GetId() == "m1");
            var m2Obj = results.Single(x => x.GetId() == "m2");

            Assert.That(m1Obj.Forces, Has.Exactly(3).Items);
            Assert.That(m2Obj.Forces, Has.Exactly(1).Items);

            Assert.That(m1Obj.Forces.OfType<ElasticForce>().Where(x => x.ForceType == Enums.ForceType.Elastic && x.Quantity == 100
                && x.Angle == 30 && x.K.Quantity == 200 && x.X.Quantity == 0.5 && x.Length.Quantity == 1), Has.Exactly(1).Items);

            Assert.That(m2Obj.Forces.OfType<ElasticForce>, Has.Exactly(0).Items);
        });
    }
}
