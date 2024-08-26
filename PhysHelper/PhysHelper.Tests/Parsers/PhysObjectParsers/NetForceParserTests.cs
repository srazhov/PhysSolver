using PhysHelper.Enums;
using PhysHelper.Parsers.PhysObjectParsers;
using PhysHelper.Scenes.SceneSettings;
using PhysHelper.SIObjects.Forces;

namespace PhysHelper.Tests.Parsers.PhysObjectParsers;

public class NetForceParserTests
{
    [Test]
    public void OneObjectWhichHasOnlyGravAndNormalForcesIsPassed_NetForceMustBeZero()
    {
        // Arrange 
        var parser = new NetForceParser();
        var results = PhysObjectHelpers.GetDefaultObjects(g: null, angle: 0, addM2: false, addGround: true, addNormalForce: true);
        var query = PhysObjectHelpers.GetDefaultSceneSettings(g: null, angle: 0, addM2: false, addGround: true);

        // Act
        parser.Parse(results, query);

        // Arrange
        Assert.Multiple(() =>
        {
            var m1Obj = results.Single(x => x.GetId() == "m1");
            Assert.That(m1Obj.Forces, Has.Exactly(3).Items);

            var m1NetForce = m1Obj.Forces.SingleOrDefault(x => x.ForceType == ForceType.Net);
            Assert.That(m1NetForce, Is.Not.Null);
            Assert.That(m1NetForce?.Magnitude, Is.EqualTo(0));
            Assert.That(m1NetForce?.Angle, Is.EqualTo(0));
        });
    }

    [TestCase(0)]
    [TestCase(180)]
    [TestCase(45)]
    [TestCase(60)]
    [TestCase(28)]
    public void TwoObjectsArePassed_M1IsMovingRightWards_MustCalculateNetForceForBothObjects(double ElasticForceAngle)
    {
        // Arrange 
        var parser = new NetForceParser();
        var results = PhysObjectHelpers.GetDefaultObjects(g: null, angle: 0, addM2: true, addGround: true, addNormalForce: true);
        var query = PhysObjectHelpers.GetDefaultSceneSettings(g: null, angle: 0, addM2: true, addGround: true);

        results.Single(x => x.GetId() == "m1").Forces.Add(new ElasticForce(100, 0.5, 1, ElasticForceAngle));

        // Act
        parser.Parse(results, query);

        // Arrange
        Assert.Multiple(() =>
        {
            var m1Obj = results.Single(x => x.GetId() == "m1");
            Assert.That(m1Obj.Forces, Has.Exactly(6).Items);

            var m1NetForce = m1Obj.Forces.SingleOrDefault(x => x.ForceType == ForceType.Net);
            Assert.That(m1NetForce, Is.Not.Null);
            Assert.That(m1NetForce?.Magnitude, Is.EqualTo(50));
            Assert.That(m1NetForce?.Angle, Is.EqualTo(ElasticForceAngle));

            var m2Obj = results.Single(x => x.GetId() == "m2");
            Assert.That(m2Obj.Forces, Has.Exactly(3).Items);

            var m2NetForce = m2Obj.Forces.SingleOrDefault(x => x.ForceType == ForceType.Net);
            Assert.That(m2NetForce, Is.Not.Null);
            Assert.That(m2NetForce?.Magnitude, Is.EqualTo(0));
            Assert.That(m2NetForce?.Angle, Is.EqualTo(0));
        });
    }

    [Test]
    public void ObjectMovesDownAtAnAngle_FrictionForceIsPassed_NetForceMustBeDirectedDownwards()
    {
        // Arrange 
        var parser = new NetForceParser();
        var results = PhysObjectHelpers.GetDefaultObjects(g: 10, angle: 30, addM2: true, addGround: true, addNormalForce: true);
        var query = PhysObjectHelpers.GetDefaultSceneSettings(g: 10, angle: 30, addM2: true, addGround: true);

        var m1Obj = results.Single(x => x.GetId() == "m1");
        m1Obj.Forces.Add(new KineticFrictionForce(0.4,
            m1Obj.Forces.Single(x => x.Mass.Quantity == 10 && x.ForceType == ForceType.Normal), 30)); // Kinetic force of m1
        m1Obj.Forces.Add(new KineticFrictionForce(0.4,
            m1Obj.Forces.Single(x => x.Mass.Quantity == 5 && x.ForceType == ForceType.Normal), 30)); // Kinetic force of m2

        var m2Obj = results.Single(x => x.GetId() == "m2");
        m2Obj.Forces.Add(new Force(25, 30, ForceType.StaticFriction)); // Static Friction force for m2

        query.ObjectsFriction?.Add(new FrictionSettings() { Angle = 30, TargetObj = "ground", SecondObj = "m1", Mu = 0.4, ObjectIsMoving = true });

        // Act
        parser.Parse(results, query);

        // Arrange
        Assert.Multiple(() =>
        {
            Assert.That(m1Obj.Forces, Has.Exactly(7).Items);

            var m1NetForce = m1Obj.Forces.SingleOrDefault(x => x.ForceType == ForceType.Net);
            Assert.That(m1NetForce, Is.Not.Null);
            Assert.That(m1NetForce?.Magnitude, Is.EqualTo(23.03847).Within(0.00001));
            Assert.That(m1NetForce?.Angle, Is.EqualTo(150));

            Assert.That(m2Obj.Forces, Has.Exactly(4).Items);

            var m2NetForce = m2Obj.Forces.SingleOrDefault(x => x.ForceType == ForceType.Net);
            Assert.That(m2NetForce, Is.Not.Null);
            Assert.That(m2NetForce?.Magnitude, Is.EqualTo(0).Within(0.00001));
            Assert.That(m2NetForce?.Angle, Is.EqualTo(0).Within(0.01));
        });
    }

    [Test]
    public void ObjectIsSittingOnAnInclinedPlane_NetForceMustBeZero()
    {
        // Arrange 
        var parser = new NetForceParser();
        var results = PhysObjectHelpers.GetDefaultObjects(g: 10, angle: 30, addM2: true, addGround: true, addNormalForce: true);
        var query = PhysObjectHelpers.GetDefaultSceneSettings(g: 10, angle: 30, addM2: true, addGround: true);

        results.Single(x => x.GetId() == "m1").Forces.Add(new Force(75, 30, ForceType.StaticFriction));
        results.Single(x => x.GetId() == "m2").Forces.Add(new Force(25, 30, ForceType.StaticFriction));

        // Act
        parser.Parse(results, query);

        // Arrange
        Assert.Multiple(() =>
        {
            var m1Obj = results.Single(x => x.GetId() == "m1");
            Assert.That(m1Obj.Forces, Has.Exactly(6).Items);

            var m1NetForce = m1Obj.Forces.SingleOrDefault(x => x.ForceType == ForceType.Net);
            Assert.That(m1NetForce, Is.Not.Null);
            Assert.That(m1NetForce?.Magnitude, Is.EqualTo(0).Within(0.00001));
            Assert.That(m1NetForce?.Angle, Is.EqualTo(0).Within(0.01));

            var m2Obj = results.Single(x => x.GetId() == "m2");
            Assert.That(m2Obj.Forces, Has.Exactly(4).Items);

            var m2NetForce = m2Obj.Forces.SingleOrDefault(x => x.ForceType == ForceType.Net);
            Assert.That(m2NetForce, Is.Not.Null);
            Assert.That(m2NetForce?.Magnitude, Is.EqualTo(0).Within(0.00001));
            Assert.That(m2NetForce?.Angle, Is.EqualTo(0).Within(0.01));
        });
    }
}