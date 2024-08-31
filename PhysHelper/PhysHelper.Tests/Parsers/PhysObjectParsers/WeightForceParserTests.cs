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
                    Mass = new QuantitySettings() {
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
                    Mass = new QuantitySettings() {
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
                    Mass = new QuantitySettings() {
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
                Assert.That(actualForce.Quantity, Is.NaN);
                Assert.That(actualForce.ForceType, Is.EqualTo(ForceType.Weight));

                Assert.That(actualForce.Mass.Quantity, Is.NaN);
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
                    Mass = new QuantitySettings() {
                        Quantity = 10,
                        SiState = SIState.Known
                    },
                    Forces = null,
                    Angle = 0
                },
                new ObjectSettings() {
                    Name = "m2",
                    Mass = new QuantitySettings() {
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

    [Test]
    public void ThreeObjectsAreAdded_MustCorrectlyCalculateWeightForces()
    {
        // Arrange
        var parser = new WeightForceParser();
        var results = new List<IPhysObject>()
        {
            new PointLikeParticle(new Mass(10), [], "m1"),
            new PointLikeParticle(new Mass(5), [], "m2"),
            new PointLikeParticle(new Mass(10), [], "m3")
        };

        var query = PhysObjectHelpers.GetDefaultSceneSettings(g: 10, angle: 0, addM2: true, addGround: true);
        query.Objects.Add(new ObjectSettings()
        {
            Name = "m3",
            Mass = new QuantitySettings()
            {
                Quantity = 10,
                SiState = SIState.Known,
            },
            Forces = null,
            Angle = 0
        });

        query.ObjectsPlacementOrder[0].Add("m3");

        // Act
        parser.Parse(results, query);

        // Assert
        var obj1Forces = results.Single(x => x.GetId() == "m1").Forces;
        var obj2Forces = results.Single(x => x.GetId() == "m2").Forces;
        var obj3Forces = results.Single(x => x.GetId() == "m3").Forces;
        Assert.Multiple(() =>
        {
            // Assert count
            Assert.That(obj1Forces, Has.Exactly(3).Items);
            Assert.That(obj2Forces, Has.Exactly(2).Items);
            Assert.That(obj3Forces, Has.Exactly(1).Items);

            // Assert ForceType
            Assert.That(obj1Forces.Select(x => x.ForceType), Is.All.EqualTo(ForceType.Weight));
            Assert.That(obj2Forces.Select(x => x.ForceType), Is.All.EqualTo(ForceType.Weight));
            Assert.That(obj3Forces.Select(x => x.ForceType), Is.All.EqualTo(ForceType.Weight));

            // Assert references
            var obj2WeightForce = obj2Forces.Single(x => x.Mass == results.Single(x => x.GetId() == "m2").Mass);
            var obj3WeightForce = obj3Forces.Single(x => x.Mass == results.Single(x => x.GetId() == "m3").Mass);

            // Assert for m1
            Assert.That(obj1Forces, Has.Exactly(1).Items.EqualTo(obj2WeightForce));
            Assert.That(obj1Forces, Has.Exactly(1).Items.EqualTo(obj3WeightForce));

            // Assert for m2
            Assert.That(obj2Forces, Has.Exactly(1).Items.EqualTo(obj3WeightForce));
        });
    }
}