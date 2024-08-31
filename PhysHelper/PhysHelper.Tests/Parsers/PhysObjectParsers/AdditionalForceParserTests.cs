using PhysHelper.Enums;
using PhysHelper.Parsers.PhysObjectParsers;
using PhysHelper.Scenes.Objects;
using PhysHelper.Scenes.SceneSettings;

namespace PhysHelper.Tests.Parsers.PhysObjectParsers;

public class AdditionalForceParserTests
{
    [Test]
    public void GroundIsPassed_MustNotAddAnyForcesToIt()
    {
        // Arrange
        var parser = new AdditionalForceParser();
        var results = PhysObjectHelpers.GetDefaultObjects(g: null, angle: 0, addM2: true, addGround: true, addNormalForce: false);
        var query = PhysObjectHelpers.GetDefaultSceneSettings(g: null, angle: 0, addM2: true, addGround: true);

        query.Objects[0].Forces = [new ForceSettings() { Quantity = 10, Angle = 30 }];
        query.Objects[1].Forces = [new ForceSettings() { Quantity = 1, Angle = 270 }];

        // Act
        parser.Parse(results, query);

        // Assert
        Assert.Multiple(() =>
        {
            var ground = results.OfType<Ground>().Single();

            Assert.That(results, Has.Exactly(3).Items);

            Assert.That(ground.Forces, Has.Exactly(0).Items);
        });
    }

    [Test]
    public void TwoObjectsArePassed_MustCorrectlyAddAdditionalForcesToThem()
    {
        // Arrange
        var parser = new AdditionalForceParser();
        var results = PhysObjectHelpers.GetDefaultObjects(g: null, angle: 0, addM2: true, addGround: true, addNormalForce: false);
        var query = PhysObjectHelpers.GetDefaultSceneSettings(g: null, angle: 0, addM2: true, addGround: true);

        query.Objects[0].Forces = [new ForceSettings() { Quantity = 10, Angle = 30, SiState = SIState.Known }];
        query.Objects[1].Forces = [new ForceSettings() { Quantity = 1, Angle = 270, SiState = SIState.Known }];

        // Act
        parser.Parse(results, query);

        // Assert
        Assert.Multiple(() =>
        {
            var m1Obj = results.Single(x => x.GetId() == "m1");
            var m2Obj = results.Single(x => x.GetId() == "m2");

            Assert.That(results, Has.Exactly(3).Items);

            Assert.That(m1Obj.Forces, Has.Exactly(3).Items);
            Assert.That(m2Obj.Forces, Has.Exactly(2).Items);

            Assert.That(m1Obj.Forces.Where(x => x.ForceType == Enums.ForceType.Additional && x.Angle == 30 && x.Quantity == 10), Has.Exactly(1).Items);
            Assert.That(m2Obj.Forces.Where(x => x.ForceType == Enums.ForceType.Additional && x.Angle == 270 && x.Quantity == 1), Has.Exactly(1).Items);
        });
    }

    // TODO: Add UnitTests
}