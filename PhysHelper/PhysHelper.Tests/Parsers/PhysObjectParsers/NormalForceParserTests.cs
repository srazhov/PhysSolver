using PhysHelper.Parsers.PhysObjectParsers;
using PhysHelper.Scenes.Objects;

namespace PhysHelper.Tests.Parsers.PhysObjectParsers;

public class NormalForceParserTests
{
    [Test]
    public void GroundIsPassed_MustNotAddAnyForceToIt()
    {
        // Arrange
        var parser = new NormalForceParser();
        var results = PhysObjectHelpers.GetDefaultObjects();
        var query = PhysObjectHelpers.GetDefaultSceneSettings();

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

            Assert.That(m1Obj.Forces.Where(x => x.ForceType == Enums.ForceType.Normal), Has.Exactly(2).Items);
            Assert.That(m2Obj.Forces.Where(x => x.ForceType == Enums.ForceType.Normal), Has.Exactly(1).Items);
        });
    }

    [Test]
    public void ObjectDoesntHaveASurfaceBelow_MustNotHaveANormalForce()
    {
        // Arrange
        var parser = new NormalForceParser();
        var results = PhysObjectHelpers.GetDefaultObjects();
        var query = PhysObjectHelpers.GetDefaultSceneSettings();

        results.RemoveAll(x => x is Ground);
        query.ObjectsPlacementOrder[0].RemoveAt(0);

        // Act
        parser.Parse(results, query);

        // Assert
        Assert.Multiple(() =>
        {
            var m1Obj = results.Single(x => x.GetId() == "m1");
            var m2Obj = results.Single(x => x.GetId() == "m2");

            Assert.That(results, Has.Exactly(2).Items);

            Assert.That(m1Obj.Forces, Has.Exactly(2).Items);
            Assert.That(m2Obj.Forces, Has.Exactly(2).Items);

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
        var results = PhysObjectHelpers.GetDefaultObjects();
        var query = PhysObjectHelpers.GetDefaultSceneSettings();
        foreach (var item in query.Objects)
        {
            item.Angle = angle;
        }

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
        var results = PhysObjectHelpers.GetDefaultObjects();
        var query = PhysObjectHelpers.GetDefaultSceneSettings();

        results.RemoveAll(x => x.GetId() == "m2");
        results[1].Forces.RemoveAll(x => x.Mass.Quantity == 5);

        query.ObjectsPlacementOrder[0].RemoveAll(x => x == "m2");
        query.Objects[0].Angle = angle;
        query.Objects.RemoveAll(x => x.Name == "m2");

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