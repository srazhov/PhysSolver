using PhysHelper.Parsers.PhysObjectParsers;
using PhysHelper.Scenes.Objects;
using PhysHelper.Scenes.SceneSettings;
using PhysHelper.SIObjects;
using PhysHelper.SIObjects.Forces;
using PhysHelper.SIObjects.Kinematics;
using PhysHelper.SIObjects.Scalars;

namespace PhysHelper.Tests.Parsers.PhysObjectParsers;

public class NormalForceParserTests
{
    [Test]
    public void GroundIsPassed_MustNotAddAnyForceToIt()
    {
        // Arrange
        var parser = new NormalForceParser();
        var results = GetDefaultObjects();
        var query = GetDefaultSceneSettings();

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
        var results = GetDefaultObjects();
        var query = GetDefaultSceneSettings();

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
        var results = GetDefaultObjects();
        var query = GetDefaultSceneSettings();
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

    private static List<IPhysObject> GetDefaultObjects()
    {
        var g = new Acceleration(Constants.Forces.g_Earth, 270);
        return [
            new Ground(),
            new PointLikeParticle(new Mass(10), [
                new Force(new Mass(10), g, 270, Enums.ForceType.Weight),
                new Force(new Mass(5), g, 270, Enums.ForceType.Weight)
            ], "m1"),
            new PointLikeParticle(new Mass(5), [
                new Force(new Mass(5), g, 270, Enums.ForceType.Weight)
            ], "m2")
        ];
    }

    private static SceneSettings GetDefaultSceneSettings()
    {
        return new SceneSettings()
        {
            Global = new GlobalSceneSettings() { },
            Ground = new GroundSettings()
            {
                Exists = true,
                Angle = 0
            },
            Objects = [
                new ObjectSettings(){
                    Name = "m1",
                    Mass = new MassSettings() { Quantity = 10, SiState = Enums.SIState.Known },
                    Angle = 0,
                    Forces = null,
                    HasKineticFriction = null
                },
                new ObjectSettings(){
                    Name = "m2",
                    Mass = new MassSettings() { Quantity = 5, SiState = Enums.SIState.Known },
                    Angle = 0,
                    Forces = null,
                    HasKineticFriction = null
                }
            ],
            ObjectsPlacementOrder = [
                ["ground", "m1", "m2"]
            ]
        };
    }
}