using PhysHelper.Enums;
using PhysHelper.Parsers.PhysObjectParsers;
using PhysHelper.Scenes.Objects;
using PhysHelper.Scenes.SceneSettings;
using PhysHelper.SIObjects.Scalars;

namespace PhysHelper.Tests.Parsers.PhysObjectParsers;

public class WeightForceParserTests
{
    [Test]
    public void G_IsNotSpecified_MustUseEarthGravAccelerationConstant()
    {
        // Arrange
        var query = new SceneSettings()
        {
            Global = null,
            Ground = null,
            Objects = [
                new ObjectSettings() {
                    Name = "m1",
                    Mass = new MassSettings() {
                        Quantity = 10,
                        SiState = SIState.Known
                    },
                    Forces = null,
                    Angle = 0
                }
            ],
            ObjectsPlacementOrder = [["m1"]],
            ObjectsFriction = null
        };

        var results = new List<IPhysObject>()
        {
            new PointLikeParticle(new Mass(10), [], "m1")
        };

        var parser = new WeightForceParser();

        // Act
        parser.Parse(results, query);

        // Assert
        var obj1Forces = results.Single().Forces;
        Assert.Multiple(() =>
        {
            Assert.That(obj1Forces, Has.Exactly(1).Items);

            var actualForce = obj1Forces.SingleOrDefault();
            if (actualForce != null)
            {
                Assert.That(actualForce.Quantity, Is.EqualTo(98.067));
                Assert.That(actualForce.Acceleration.Quantity, Is.EqualTo(9.8067));
                Assert.That(actualForce.Acceleration.Angle, Is.EqualTo(270));
            }
        });
    }

    [Test]
    public void G_IsSpecified_MustUseCustomGravAccelerationConstant()
    {
        // Arrange
        var query = new SceneSettings()
        {
            Global = new GlobalSceneSettings()
            {
                G = 12
            },
            Ground = null,
            Objects = [
                new ObjectSettings() {
                    Name = "m1",
                    Mass = new MassSettings() {
                        Quantity = 10,
                        SiState = SIState.Known
                    },
                    Forces = null,
                    Angle = 0
                }
            ],
            ObjectsPlacementOrder = [["m1"]],
            ObjectsFriction = null
        };

        var results = new List<IPhysObject>()
        {
            new PointLikeParticle(new Mass(10), [], "m1")
        };

        var parser = new WeightForceParser();

        // Act
        parser.Parse(results, query);

        // Assert
        var obj1Forces = results.Single().Forces;
        Assert.Multiple(() =>
        {
            Assert.That(obj1Forces, Has.Exactly(1).Items);

            var actualForce = obj1Forces.SingleOrDefault();
            if (actualForce != null)
            {
                Assert.That(actualForce.Quantity, Is.EqualTo(120));
                Assert.That(actualForce.Acceleration.Quantity, Is.EqualTo(12));
                Assert.That(actualForce.Acceleration.Angle, Is.EqualTo(270));
            }
        });
    }

    [Test]
    public void MassIsNotKnown_WeightForceMustBeUnimportant_ItsQuantityShouldBe0()
    {
        // Arrange
        var query = new SceneSettings()
        {
            Global = null,
            Ground = null,
            Objects = [
                new ObjectSettings() {
                    Name = "m1",
                    Mass = new MassSettings() {
                        Quantity = 0,
                        SiState = SIState.NeedsToBeFound
                    },
                    Forces = null,
                    Angle = 0
                }
            ],
            ObjectsPlacementOrder = [["m1"]],
            ObjectsFriction = null
        };

        var results = new List<IPhysObject>()
        {
            new PointLikeParticle(new Mass(0) { SIState = SIState.NeedsToBeFound}, [], "m1")
        };

        var parser = new WeightForceParser();

        // Act
        parser.Parse(results, query);

        // Assert
        var obj1Forces = results.Single().Forces;
        Assert.Multiple(() =>
        {
            Assert.That(obj1Forces, Has.Exactly(1).Items);

            var actualForce = obj1Forces.SingleOrDefault();
            if (actualForce != null)
            {
                Assert.That(actualForce.SIState, Is.EqualTo(SIState.Unimportant));
                Assert.That(actualForce.Quantity, Is.EqualTo(0));
                Assert.That(actualForce.ForceType, Is.EqualTo(ForceType.Weight));

                Assert.That(actualForce.Mass.Quantity, Is.EqualTo(0));
                Assert.That(actualForce.Mass.SIState, Is.EqualTo(SIState.NeedsToBeFound));

                Assert.That(actualForce.Acceleration.Quantity, Is.EqualTo(9.8067));
                Assert.That(actualForce.Acceleration.Angle, Is.EqualTo(270));
            }
        });
    }

    [Test]
    public void ObjectIsPlacedOnTopOfAnother_M1ShouldContain2Forces_M2ShouldContainOne()
    {
        // Arrange
        var query = new SceneSettings()
        {
            Global = new GlobalSceneSettings()
            {
                G = 10
            },
            Ground = new GroundSettings()
            {
                Exists = true,
                Angle = 0,
            },
            Objects = [
                new ObjectSettings() {
                    Name = "m1",
                    Mass = new MassSettings() {
                        Quantity = 10,
                        SiState = SIState.Known
                    },
                    Forces = null,
                    Angle = 0
                },
                new ObjectSettings() {
                    Name = "m2",
                    Mass = new MassSettings() {
                        Quantity = 15,
                        SiState = SIState.Known
                    },
                    Forces = null,
                    Angle = 0
                }
            ],
            ObjectsPlacementOrder = [["ground", "m1", "m2"]],
            ObjectsFriction = null
        };

        var results = new List<IPhysObject>()
        {
            new Ground(),
            new PointLikeParticle(new Mass(10), [], "m1"),
            new PointLikeParticle(new Mass(15), [], "m2"),
        };

        var parser = new WeightForceParser();

        // Act
        parser.Parse(results, query);

        // Assert
        var obj1Forces = results.Single(x => x.GetId() == "m1").Forces;
        var obj2Forces = results.Single(x => x.GetId() == "m2").Forces;
        Assert.Multiple(() =>
        {
            Assert.That(obj1Forces, Has.Exactly(2).Items);
            Assert.That(obj2Forces, Has.Exactly(1).Items);

            if (obj1Forces.Count == 2)
            {
                var actualForce1_Obj1 = obj1Forces.SingleOrDefault(x => x.Quantity == 100);
                if (actualForce1_Obj1 != null)
                {
                    Assert.That(actualForce1_Obj1.Quantity, Is.EqualTo(100));
                    Assert.That(actualForce1_Obj1.ForceType, Is.EqualTo(ForceType.Weight));
                    Assert.That(actualForce1_Obj1.Acceleration.Quantity, Is.EqualTo(10));
                    Assert.That(actualForce1_Obj1.Acceleration.Angle, Is.EqualTo(270));
                }

                var actualForce2_Obj1 = obj1Forces.SingleOrDefault(x => x.Quantity == 150);
                if (actualForce2_Obj1 != null)
                {
                    Assert.That(actualForce2_Obj1.Quantity, Is.EqualTo(150));
                    Assert.That(actualForce2_Obj1.ForceType, Is.EqualTo(ForceType.Weight));
                    Assert.That(actualForce2_Obj1.Acceleration.Quantity, Is.EqualTo(10));
                    Assert.That(actualForce2_Obj1.Acceleration.Angle, Is.EqualTo(270));
                }
            }

            var actualForce_Obj2 = obj2Forces.SingleOrDefault();
            if (actualForce_Obj2 != null)
            {
                Assert.That(actualForce_Obj2.Quantity, Is.EqualTo(150));
                Assert.That(actualForce_Obj2.ForceType, Is.EqualTo(ForceType.Weight));
                Assert.That(actualForce_Obj2.Acceleration.Quantity, Is.EqualTo(10));
                Assert.That(actualForce_Obj2.Acceleration.Angle, Is.EqualTo(270));
            }
        });
    }
}