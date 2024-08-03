using PhysHelper.Parsers.PhysObjectParsers;
using PhysHelper.Scenes.Objects;
using PhysHelper.Scenes.SceneSettings;

namespace PhysHelper.Tests.Parsers.PhysObjectParsers;

public class AdditionalForceParserTests
{
    [Test]
    public void GroundIsPassed_MustOnlyAddForcesToTheSpecifiedObjects()
    {
        // Arrange
        var parser = new AdditionalForceParser();
        var results = PhysObjectHelpers.GetDefaultObjects();
        var query = PhysObjectHelpers.GetDefaultSceneSettings();

        query.Objects[0].Forces = [new ForceSettings() { Quantity = 10, Angle = 30 }];
        query.Objects[1].Forces = [new ForceSettings() { Quantity = 1, Angle = 270 }];

        // Act
        parser.Parse(results, query);

        // Assert
        Assert.Multiple(() =>
        {
            var ground = results.OfType<Ground>().Single();
            var m1Obj = results.Single(x => x.GetId() == "m1");
            var m2Obj = results.Single(x => x.GetId() == "m2");

            Assert.That(results, Has.Exactly(3).Items);

            Assert.That(ground.Forces, Has.Exactly(0).Items);
            Assert.That(m1Obj.Forces, Has.Exactly(3).Items);
            Assert.That(m2Obj.Forces, Has.Exactly(2).Items);

            Assert.That(m1Obj.Forces.Where(x => x.ForceType == Enums.ForceType.Additional && x.Angle == 30 && x.Quantity == 10), Has.Exactly(1).Items);
            Assert.That(m2Obj.Forces.Where(x => x.ForceType == Enums.ForceType.Additional && x.Angle == 270 && x.Quantity == 1), Has.Exactly(1).Items);
        });
    }
}