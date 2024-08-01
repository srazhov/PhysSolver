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
                        SiState = Enums.SIState.Known
                    },
                    HasKineticFriction = null,
                    Forces = null,
                    Angle = 0
                }
            ]
        };

        var results = new List<IPhysObject>()
        {
            new PointLikeParticle(new Mass(10), [], "m1")
        };

        var parser = new WeightForceParser();

        // Act
        parser.Parse(results, query);
        var obj1Forces = results.Single().Forces;

        // Assert
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
                        SiState = Enums.SIState.Known
                    },
                    HasKineticFriction = null,
                    Forces = null,
                    Angle = 0
                }
            ]
        };

        var results = new List<IPhysObject>()
        {
            new PointLikeParticle(new Mass(10), [], "m1")
        };

        var parser = new WeightForceParser();

        // Act
        parser.Parse(results, query);
        var obj1Forces = results.Single().Forces;

        // Assert
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
                    HasKineticFriction = null,
                    Forces = null,
                    Angle = 0
                }
            ]
        };

        var results = new List<IPhysObject>()
        {
            new PointLikeParticle(new Mass(0) { SIState = SIState.NeedsToBeFound}, [], "m1")
        };

        var parser = new WeightForceParser();

        // Act
        parser.Parse(results, query);
        var obj1Forces = results.Single().Forces;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(obj1Forces, Has.Exactly(1).Items);

            var actualForce = obj1Forces.SingleOrDefault();
            if (actualForce != null)
            {
                Assert.That(actualForce.SIState, Is.EqualTo(SIState.Unimportant));
                Assert.That(actualForce.Quantity, Is.EqualTo(0));

                Assert.That(actualForce.Mass.Quantity, Is.EqualTo(0));
                Assert.That(actualForce.Mass.SIState, Is.EqualTo(SIState.NeedsToBeFound));

                Assert.That(actualForce.Acceleration.Quantity, Is.EqualTo(9.8067));
                Assert.That(actualForce.Acceleration.Angle, Is.EqualTo(270));
            }
        });
    }
}