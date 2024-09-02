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

        query.Objects[0].Forces = [new ForceSettings() { Quantity = 10, Angle = 30, SiState = SIState.Known, Acceleration = null, IsNetForce = false }];
        query.Objects[1].Forces = [new ForceSettings() { Quantity = 1, Angle = 270, SiState = SIState.Known, Acceleration = null, IsNetForce = false }];

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

        query.Objects[0].Forces = [new ForceSettings() { Quantity = 10, Angle = 30, SiState = SIState.Known, Acceleration = null, IsNetForce = false }];
        query.Objects[1].Forces = [new ForceSettings() { Quantity = 1, Angle = 270, SiState = SIState.Known, Acceleration = null, IsNetForce = false }];

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

            var m1Force = PhysObjectHelpers.AssertOneForce(m1Obj.Forces, ForceType.Additional, 10, 30);
            var m2Force = PhysObjectHelpers.AssertOneForce(m2Obj.Forces, ForceType.Additional, 1, 270);

            Assert.That(m1Force?.Mass, Is.EqualTo(m1Obj.Mass));
            Assert.That(m2Force?.Mass, Is.EqualTo(m2Obj.Mass));

            Assert.That(m1Force?.Acceleration.Quantity, Is.EqualTo(1));
            Assert.That(m2Force?.Acceleration.Quantity, Is.EqualTo(0.2).Within(0.00001));
            Assert.That(m2Force?.Acceleration.SIState, Is.EqualTo(SIState.Known));
        });
    }

    [Test]
    public void M1DoesntHaveForcesSetting_MustNotThrowAnException()
    {
        // Arrange
        var parser = new AdditionalForceParser();
        var results = PhysObjectHelpers.GetDefaultObjects(g: null, angle: 0, addM2: true, addGround: true, addNormalForce: false);
        var query = PhysObjectHelpers.GetDefaultSceneSettings(g: null, angle: 0, addM2: true, addGround: true);

        query.Objects[0].Forces = null;
        query.Objects[1].Forces = [new ForceSettings() { Quantity = 1, Angle = 270, SiState = SIState.Known, Acceleration = null, IsNetForce = false }];

        // Act
        parser.Parse(results, query);

        // Assert
        Assert.Multiple(() =>
        {
            var m1Obj = results.Single(x => x.GetId() == "m1");
            var m2Obj = results.Single(x => x.GetId() == "m2");

            Assert.That(results, Has.Exactly(3).Items);

            Assert.That(m1Obj.Forces, Has.Exactly(2).Items);
            Assert.That(m2Obj.Forces, Has.Exactly(2).Items);

            Assert.That(m1Obj.Forces.Where(x => x.ForceType == ForceType.Additional || x.ForceType == ForceType.Net), Has.Exactly(0).Items);
            PhysObjectHelpers.AssertOneForce(m2Obj.Forces, ForceType.Additional, 1, 270);
        });
    }

    [Test]
    public void M1DoesntHaveForcesSetting_M2HasAccelerationSpecified_MustIgnoreSpecifiedQuantity_And_CorrectlyAddANewForce()
    {
        // Arrange
        var parser = new AdditionalForceParser();
        var results = PhysObjectHelpers.GetDefaultObjects(g: null, angle: 0, addM2: true, addGround: true, addNormalForce: true);
        var query = PhysObjectHelpers.GetDefaultSceneSettings(g: null, angle: 0, addM2: true, addGround: true);

        query.Objects[0].Forces = null;
        query.Objects[1].Forces = [new ForceSettings() { Quantity = 10, Angle = 35, SiState = SIState.Known, IsNetForce = false,
            Acceleration = new QuantitySettings() { Quantity = 10, SiState = SIState.Known } }
        ];

        // Act
        parser.Parse(results, query);

        // Assert
        Assert.Multiple(() =>
        {
            var m1Obj = results.Single(x => x.GetId() == "m1");
            var m2Obj = results.Single(x => x.GetId() == "m2");

            Assert.That(results, Has.Exactly(3).Items);

            Assert.That(m1Obj.Forces, Has.Exactly(4).Items);
            Assert.That(m2Obj.Forces, Has.Exactly(3).Items);

            Assert.That(m1Obj.Forces.Where(x => x.ForceType == ForceType.Additional || x.ForceType == ForceType.Net), Has.Exactly(0).Items);
            var m2Force = PhysObjectHelpers.AssertOneForce(m2Obj.Forces, ForceType.Additional, 50, 35);

            Assert.That(m2Force?.Mass, Is.EqualTo(m2Obj.Mass));
            Assert.That(m2Force?.Acceleration.Quantity, Is.EqualTo(10));
            Assert.That(m2Force?.Acceleration.SIState, Is.EqualTo(SIState.Known));
        });
    }

    [Test]
    public void M2HasNetForce_MustAddNetForceInsteadOfAdditional()
    {
        // Arrange
        var parser = new AdditionalForceParser();
        var results = PhysObjectHelpers.GetDefaultObjects(g: null, angle: 0, addM2: true, addGround: true, addNormalForce: true);
        var query = PhysObjectHelpers.GetDefaultSceneSettings(g: null, angle: 0, addM2: true, addGround: true);

        query.Objects[0].Forces = null;
        query.Objects[1].Forces = [new ForceSettings() { Quantity = 10, Angle = 35, SiState = SIState.NeedsToBeFound, IsNetForce = true,
            Acceleration = new QuantitySettings() { Quantity = 10, SiState = SIState.NeedsToBeFound } }
        ];

        // Act
        parser.Parse(results, query);

        // Assert
        Assert.Multiple(() =>
        {
            var m1Obj = results.Single(x => x.GetId() == "m1");
            var m2Obj = results.Single(x => x.GetId() == "m2");

            Assert.That(results, Has.Exactly(3).Items);

            Assert.That(m1Obj.Forces, Has.Exactly(4).Items);
            Assert.That(m2Obj.Forces, Has.Exactly(3).Items);

            Assert.That(m1Obj.Forces.Where(x => x.ForceType == ForceType.Additional || x.ForceType == ForceType.Net), Has.Exactly(0).Items);
            var m2Force = PhysObjectHelpers.AssertOneForce(m2Obj.Forces, ForceType.Net, double.NaN, 35);

            Assert.That(m2Force?.SIState, Is.EqualTo(SIState.NeedsToBeFound));

            Assert.That(m2Force?.Mass, Is.EqualTo(m2Obj.Mass));

            Assert.That(m2Force?.Acceleration.Quantity, Is.NaN);
            Assert.That(m2Force?.Acceleration.SIState, Is.EqualTo(SIState.NeedsToBeFound));
        });
    }
}
