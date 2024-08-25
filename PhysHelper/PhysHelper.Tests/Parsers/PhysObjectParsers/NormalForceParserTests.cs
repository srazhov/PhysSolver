using PhysHelper.Parsers.PhysObjectParsers;
using PhysHelper.Scenes.Objects;

namespace PhysHelper.Tests.Parsers.PhysObjectParsers;

public class NormalForceParserTests
{
    [Test]
    public void ObjectDoesntHaveASurfaceBelow_MustNotHaveANormalForce()
    {
        // Arrange
        var parser = new NormalForceParser();
        var results = PhysObjectHelpers.GetDefaultObjects(g: null, angle: 0, addM2: true, addGround: false, addNormalForce: false);
        var query = PhysObjectHelpers.GetDefaultSceneSettings(g: null, angle: 0, addM2: true, addGround: false);

        // Act
        parser.Parse(results, query);

        // Assert
        Assert.Multiple(() =>
        {
            var m1Obj = results.Single(x => x.GetId() == "m1");
            var m2Obj = results.Single(x => x.GetId() == "m2");

            Assert.That(results, Has.Exactly(2).Items);

            Assert.That(m1Obj.Forces, Has.Exactly(2).Items); // m1 has (m1 + m2)g. But not N
            Assert.That(m2Obj.Forces, Has.Exactly(2).Items); // m2 has m2g + N = 0

            Assert.That(m1Obj.Forces.Where(x => x.ForceType == Enums.ForceType.Normal), Has.Exactly(0).Items);
            Assert.That(m2Obj.Forces.Where(x => x.ForceType == Enums.ForceType.Normal), Has.Exactly(1).Items);
        });
    }

    [TestCase(0, 90)]
    [TestCase(45, 135)]
    [TestCase(180, 270)]
    public void ObjectIsSittingOnAnAngledSurface_NormalForceMustBePerpendicular(double angle, double expectedNormalForceAngle)
    {
        // Arrange
        var parser = new NormalForceParser();
        var results = PhysObjectHelpers.GetDefaultObjects(g: null, angle: angle, addM2: true, addGround: true, addNormalForce: false);
        var query = PhysObjectHelpers.GetDefaultSceneSettings(g: null, angle: angle, addM2: true, addGround: true);

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
            Assert.That(m1Obj.Forces, Has.Exactly(4).Items);
            Assert.That(m2Obj.Forces, Has.Exactly(2).Items);

            Assert.That(m1Obj.Forces.Where(x => x.ForceType == Enums.ForceType.Normal && x.Angle == expectedNormalForceAngle), Has.Exactly(2).Items);
            Assert.That(m2Obj.Forces.Where(x => x.ForceType == Enums.ForceType.Normal && x.Angle == expectedNormalForceAngle), Has.Exactly(1).Items);
        });
    }

    [TestCase(0, 98.067)]
    [TestCase(30, 84.928513)]
    [TestCase(45, 69.343840)]
    public void ObjectIsSittingOnAnAngledSurfaceWithStaticFricton_NormalForceMustHaveCorrectQuantity(double angle, double expectedNormalForce)
    {
        // Arrange
        var parser = new NormalForceParser();
        var results = PhysObjectHelpers.GetDefaultObjects(g: null, angle: angle, addM2: false, addGround: true, addNormalForce: false);
        var query = PhysObjectHelpers.GetDefaultSceneSettings(g: null, angle: angle, addM2: false, addGround: true);

        // Act
        parser.Parse(results, query);

        // Assert
        Assert.Multiple(() =>
        {
            var m1Obj = results.Single(x => x.GetId() == "m1");

            Assert.That(m1Obj.Forces, Has.Exactly(2).Items);

            var normalForce = m1Obj.Forces.FirstOrDefault(x => x.ForceType == Enums.ForceType.Normal);
            Assert.That(normalForce, Is.Not.Null);
            Assert.That(normalForce?.Magnitude, Is.EqualTo(expectedNormalForce).Within(0.00001));
        });
    }
}