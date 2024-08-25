using PhysHelper.Enums;
using PhysHelper.Parsers.PhysObjectParsers;
using PhysHelper.Scenes.Objects;
using PhysHelper.Scenes.SceneSettings;
using PhysHelper.SIObjects.Forces;

namespace PhysHelper.Tests.Parsers.PhysObjectParsers;

public class StaticForceParserTests
{
    [Test]
    public void GroundIsPassed_MustNotAddAnyForcesToIt()
    {
        // Arrange
        var parser = new StaticForceParser();
        var results = PhysObjectHelpers.GetDefaultObjects(g: null, angle: 0, addM2: false, addGround: true, addNormalForce: true);
        var query = PhysObjectHelpers.GetDefaultSceneSettings(g: null, angle: 0, addM2: false, addGround: true);

        // Act
        parser.Parse(results, query);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(results, Has.Exactly(2).Items);

            var groundObjs = results.OfType<Ground>();
            Assert.That(groundObjs, Has.Exactly(1).Items);
            if (groundObjs.Count() == 1)
            {
                var obj = groundObjs.Single();
                Assert.That(obj.Forces, Has.Exactly(0).Items);
            }
        });
    }

    [Test]
    public void ObjectsLieOnAFlatPlane_StaticFrictionMustEqualToNormalForce()
    {
        // Arrange
        var parser = new StaticForceParser();
        var results = PhysObjectHelpers.GetDefaultObjects(g: 10, angle: 0, addM2: true, addGround: true, addNormalForce: true);
        var query = PhysObjectHelpers.GetDefaultSceneSettings(g: 10, angle: 0, addM2: true, addGround: true);

        query.Objects.Single(x => x.Name == "m1").Forces?.Add(new ForceSettings() { Angle = 0, Quantity = 2 });
        query.ObjectsFriction = [
            new FrictionSettings() { Angle = 180, Mu = 0, TargetObj = "m1", SecondObj = "ground", ObjectIsMoving = false }
        ];

        results.Single(x => x.GetId() == "m1").Forces.Add(new Force(10, 0, ForceType.Additional));

        // Act
        parser.Parse(results, query);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(results, Has.Exactly(3).Items);

            var m1Obj = results.Single(x => x.GetId() == "m1");
            var m2Obj = results.Single(x => x.GetId() == "m2");
            Assert.That(m1Obj.Forces, Has.Exactly(7).Items);
            Assert.That(m2Obj.Forces, Has.Exactly(2).Items);

            var m1StaticFrForces = m1Obj.Forces.Where(x => x.ForceType == ForceType.StaticFriction);
            var m2StaticFrForces = m2Obj.Forces.Where(x => x.ForceType == ForceType.StaticFriction);

            // Assert Static Friction Forces Count for each object
            // m2 should not have static force
            Assert.That(m1StaticFrForces, Has.Exactly(2).Items);
            Assert.That(m2StaticFrForces, Has.Exactly(0).Items);

            // Assert static force Angle
            Assert.That(m1StaticFrForces.Where(x => x.Angle == 180), Has.Exactly(2).Items);

            // Assert m1's static force masses
            Assert.That(m1StaticFrForces.Where(x => x.Mass.Quantity == 10), Has.Exactly(1).Items);
            Assert.That(m1StaticFrForces.Where(x => x.Mass.Quantity == 5), Has.Exactly(1).Items);

            // Assert m1's static force quantity
            Assert.That(m1StaticFrForces.Where(x => x.Quantity == 100), Has.Exactly(1).Items);
            Assert.That(m1StaticFrForces.Where(x => x.Quantity == 50), Has.Exactly(1).Items);
        });
    }

    // TODO
    [Test]
    public void ObjectLiesOnAnInclinedPlane_StaticFrictionMustEqualToNormalForceTimesAngle()
    {
        // Arrange
        var parser = new StaticForceParser();
        var results = PhysObjectHelpers.GetDefaultObjects(g: 10, angle: 30, addM2: false, addGround: true, addNormalForce: true);
        var query = PhysObjectHelpers.GetDefaultSceneSettings(g: 10, angle: 30, addM2: false, addGround: true);

        query.ObjectsFriction = [
            new FrictionSettings() { Angle = 30, Mu = 0, TargetObj = "m1", SecondObj = "ground", ObjectIsMoving = false }
        ];

        // Act
        parser.Parse(results, query);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(results, Has.Exactly(2).Items);

            var m1Obj = results.Single(x => x.GetId() == "m1");
            Assert.That(m1Obj.Forces, Has.Exactly(3).Items);

            var m1StaticFrForces = m1Obj.Forces.Where(x => x.ForceType == ForceType.StaticFriction);

            // Assert Static Friction Forces Count for m1
            Assert.That(m1StaticFrForces, Has.Exactly(1).Items);

            // Assert static force Angle
            Assert.That(m1StaticFrForces.Where(x => x.Angle == 30), Has.Exactly(1).Items);

            // Assert m1's static force masses
            Assert.That(m1StaticFrForces.Where(x => x.Mass.Quantity == 10), Has.Exactly(1).Items);

            // Assert m1's static force quantity
            Assert.That(m1StaticFrForces.Where(x => x.Quantity == 50), Has.Exactly(1).Items);
        });
    }
}
