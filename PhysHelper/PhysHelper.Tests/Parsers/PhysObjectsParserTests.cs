using PhysHelper.Enums;
using PhysHelper.Parsers;
using PhysHelper.Scenes.SceneSettings;
using PhysHelper.SIObjects;

namespace PhysHelper.Tests.Parsers;

public class PhysObjectsParserTests
{
    [Test]
    public void QueryWithTwoDefinedObjectsSittingOnAFlatPlaneIsPassed_MustParse2ObjectsForces()
    {
        // Arrange
        var query = PhysObjectHelpers.GetDefaultSceneSettings(g: 9.8, angle: 0, addM2: true, addGround: true);

        // Act
        var results = PhysObjectsParser.Parse(query);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(results, Has.Exactly(3).Items);

            // Assert there is a Ground object
            var ground = results.SingleOrDefault(x => x.GetId() == Constants.GroundId);
            Assert.That(ground, Is.Not.Null);
            Assert.That(ground?.Forces, Has.Exactly(0).Items);

            // Assert m1 Forces
            var m1 = results.SingleOrDefault(x => x.GetId() == "m1");
            Assert.That(m1, Is.Not.Null);
            Assert.That(m1?.Forces, Has.Exactly(5).Items);
            Assert.That(m1?.Forces.Where(x => x.ForceType == ForceType.Weight), Has.Exactly(2).Items);
            Assert.That(m1?.Forces.Where(x => x.ForceType == ForceType.Normal), Has.Exactly(2).Items);

            var m1NetForce = m1?.Forces.SingleOrDefault(x => x.ForceType == ForceType.Net);
            Assert.That(m1NetForce, Is.Not.Null);
            Assert.That(m1NetForce?.Quantity, Is.EqualTo(0));

            // Assert m2 Forces
            var m2 = results.SingleOrDefault(x => x.GetId() == "m2");
            Assert.That(m2, Is.Not.Null);
            Assert.That(m2?.Forces, Has.Exactly(3).Items);
            Assert.That(m2?.Forces.Where(x => x.ForceType == ForceType.Weight), Has.Exactly(1).Items);
            Assert.That(m2?.Forces.Where(x => x.ForceType == ForceType.Normal), Has.Exactly(1).Items);

            var m2NetForce = m2?.Forces.SingleOrDefault(x => x.ForceType == ForceType.Net);
            Assert.That(m2NetForce, Is.Not.Null);
            Assert.That(m2NetForce?.Quantity, Is.EqualTo(0));
        });
    }

    [Test]
    public void QueryWithTwoDefinedObjectsAtAnAngleWithAdditionalForcesIsPassed_MustParse2ObjectsForces()
    {
        // Arrange
        var query = new SceneSettings()
        {
            Global = new GlobalSceneSettings() { G = 9.8 },
            Ground = new GroundSettings() { Exists = true, Angle = 15 },
            Objects = [
                new ObjectSettings()
                {
                    Name = "m1",
                    Mass = new QuantitySettings() { Quantity = 0.75, SiState = SIState.Known },
                    Angle = 15,
                    ElasticForce = null,
                    Forces = [ new ForceSettings() { Angle = 15, Quantity = 0, SiState = SIState.Known, IsNetForce = true,
                        Acceleration = new QuantitySettings() { Quantity = 1.14, SiState = SIState.Known } }]
                },
                new ObjectSettings()
                {
                    Name = "m2",
                    Mass = new QuantitySettings() { Quantity = 0.5, SiState = SIState.Known },
                    Angle = 15,
                    ElasticForce = null,
                    Forces = [
                        new ForceSettings() { Angle = 15, Quantity = 0, SiState = SIState.Known, IsNetForce = true,
                            Acceleration = new QuantitySettings() { Quantity = 1.14, SiState = SIState.Known } },
                        new ForceSettings() { Angle = 15, Quantity = 0, SiState = SIState.NeedsToBeFound, IsNetForce = false, Acceleration = null }
                    ]
                },
            ],
            ObjectsPlacementOrder = [["ground", "m1"], ["ground", "m2"]],
            ObjectsFriction = [new FrictionSettings() { Angle = 195, Mu = 0.18, TargetObj = "m1", SecondObj = "ground", ObjectIsMoving = true }],
            Tensions = [new TensionSettings() { TargetObj = "m2", SecondObj = "m1", TargetObjAngle = 195, SecondObjAngle = 15 }]
        };

        // Act
        var results = PhysObjectsParser.Parse(query);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(results, Has.Exactly(3).Items);

            // Assert there is a Ground object
            var ground = results.SingleOrDefault(x => x.GetId() == Constants.GroundId);
            Assert.That(ground, Is.Not.Null);
            Assert.That(ground?.Forces, Has.Exactly(0).Items);

            // Assert m1 Forces
            var m1 = results.SingleOrDefault(x => x.GetId() == "m1");
            Assert.That(m1, Is.Not.Null);
            Assert.That(m1?.Forces, Has.Exactly(5).Items);

            PhysObjectHelpers.AssertOneForce(m1?.Forces, ForceType.Weight, 7.35, 270);
            PhysObjectHelpers.AssertOneForce(m1?.Forces, ForceType.Normal, 7.09955, 105);
            PhysObjectHelpers.AssertOneForce(m1?.Forces, ForceType.KineticFriction, 1.277919, 195);
            PhysObjectHelpers.AssertOneForce(m1?.Forces, ForceType.Tension, 4.03524, 15);
            PhysObjectHelpers.AssertOneForce(m1?.Forces, ForceType.Net, -0.855, 15);

            // Assert m2 Forces
            var m2 = results.SingleOrDefault(x => x.GetId() == "m2");
            Assert.That(m2, Is.Not.Null);
            Assert.That(m2?.Forces, Has.Exactly(5).Items);

            PhysObjectHelpers.AssertOneForce(m2?.Forces, ForceType.Weight, 4.9, 270);
            PhysObjectHelpers.AssertOneForce(m2?.Forces, ForceType.Normal, 4.73303, 105);
            PhysObjectHelpers.AssertOneForce(m2?.Forces, ForceType.Tension, -4.03524, 195);
            PhysObjectHelpers.AssertOneForce(m2?.Forces, ForceType.Net, -0.57, 15);
            PhysObjectHelpers.AssertOneForce(m2?.Forces, ForceType.Additional, double.NaN, 15);
        });
    }
}