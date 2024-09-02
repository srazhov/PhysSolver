using PhysHelper.Enums;
using PhysHelper.Parsers.PhysObjectParsers;
using PhysHelper.Scenes.Objects;
using PhysHelper.Scenes.SceneSettings;
using PhysHelper.SIObjects.Forces;
using PhysHelper.SIObjects.Kinematics;
using PhysHelper.SIObjects.Scalars;

namespace PhysHelper.Tests.Parsers.PhysObjectParsers;

public class TensionForceParserTests
{
    [Test]
    public void Query_Tensions_IsNull_MustNotAddAnyNewForce()
    {
        // Arrange
        var parser = new TensionForceParser();
        var results = PhysObjectHelpers.GetDefaultObjects(g: null, angle: 0, addM2: true, addGround: true, addNormalForce: true);
        var query = PhysObjectHelpers.GetDefaultSceneSettings(g: null, angle: 0, addM2: true, addGround: true);

        query.Tensions = null;

        // Act
        parser.Parse(results, query);

        // Assert
        var m1 = results.Single(x => x.GetId() == "m1");
        var m2 = results.Single(x => x.GetId() == "m2");

        Assert.Multiple(() =>
        {
            Assert.That(m1.Forces.OfType<TensionForce>(), Has.Exactly(0).Items);
            Assert.That(m2.Forces.OfType<TensionForce>(), Has.Exactly(0).Items);
        });
    }

    [TestCase("m1", 180, 0, 90, 0, Description = "Tension and Net Forces are directed to Right, KinFriction to Left. Target object is m1")]
    [TestCase("m2", 180, 90, 0, 0, Description = "Tension and Net Forces are directed to Right, KinFriction to Left. Target object is m2")]
    [TestCase("m1", 0, 180, 90, 180, Description = "Tension and Net Forces are directed to Left, KinFriction to Right. Target object is m1")]
    [TestCase("m2", 0, 90, 180, 180, Description = "Tension and Net Forces are directed to Left, KinFriction to Right. Target object is m2")]
    public void TensionBetweenHorizontalM1AndVerticalM2IsPassed_MustReturnCorrectTensionForces(string targetObj, double kinAngle,
        double targetAngle, double secondAngle, double netAngle)
    {
        // Arrange
        var parser = new TensionForceParser();
        var results = new List<IPhysObject>()
        {
            new Ground(),
            GetProperObject("m1", 10, true, 3.92, netAngle),
            GetProperObject("m2", 5, false, 3.92, 270)
        };

        results[1].Forces.Add(new KineticFrictionForce(0.3, results[1].Forces[1], kinAngle));

        var query = new SceneSettings()
        {
            // No other setting than Tensions is needed for this Parser
            Objects = [],
            ObjectsPlacementOrder = [],
            Tensions =
            [
                new TensionSettings() { TargetObj = targetObj, SecondObj = targetObj == "m1" ? "m2" : "m1",
                    TargetObjAngle = targetAngle, SecondObjAngle = secondAngle }
            ]
        };

        // Act
        parser.Parse(results, query);

        // Assert
        var m1 = results.Single(x => x.GetId() == "m1");
        var m2 = results.Single(x => x.GetId() == "m2");

        Assert.Multiple(() =>
        {
            PhysObjectHelpers.AssertOneForce(m1.Forces, ForceType.Tension, 68.6, netAngle);
            PhysObjectHelpers.AssertOneForce(m2.Forces, ForceType.Tension, 68.6, 90);
        });
    }

    private static IPhysObject GetProperObject(string name, double mass, bool addNormalForce = true, double netAcceleration = 0, double netAngle = 0)
    {
        var m = new Mass(mass);
        var g = new Acceleration(9.8, 270);
        IPhysObject result = new PointLikeParticle(m, [
            new Force(m, g, 270, ForceType.Weight),
        ], name);

        if (addNormalForce)
        {
            result.Forces.Add(new Force(m, g, 90, ForceType.Normal));
        }

        if (netAcceleration != 0)
        {
            var a = new Acceleration(netAcceleration, netAngle);
            result.Forces.Add(new Force(m, a, netAngle, ForceType.Net));
        }

        return result;
    }
}
