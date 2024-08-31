using PhysHelper.Enums;
using PhysHelper.Parsers.PhysObjectParsers;
using PhysHelper.Scenes.Objects;
using PhysHelper.Scenes.SceneSettings;
using PhysHelper.SIObjects;
using PhysHelper.SIObjects.Forces;
using PhysHelper.SIObjects.Kinematics;
using PhysHelper.SIObjects.Scalars;

namespace PhysHelper.Tests.Parsers.PhysObjectParsers;

public class KineticFrictionForceParserTests
{
    [Test]
    public void ObjectHasNoNormalForce_MustSkipIt()
    {
        // Arrange
        var parser = new KineticFrictionForceParser();
        var results = AddKineticFrictionForceSpecificDefaultObjects(addM2: false, addM3: false, hasNormalForce: false);
        var query = AddKineticFrictionForceSpecificDefaultSceneSettings(addM2: false, addM3: false);

        // Act
        parser.Parse(results, query);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(results, Has.Exactly(2).Items);

            var m1Obj = results.Single(x => x.GetId() == "m1");
            Assert.That(m1Obj.Forces, Has.Exactly(2).Items);

            var m1KineticFrictionForces = m1Obj.Forces.OfType<KineticFrictionForce>();
            Assert.That(m1Obj.Forces.OfType<KineticFrictionForce>(), Has.Exactly(0).Items);
        });
    }

    [Test]
    public void TwoObjectsThatAreNotMovingPassed_MustSkipThem()
    {
        // Arrange
        var parser = new KineticFrictionForceParser();
        var results = AddKineticFrictionForceSpecificDefaultObjects(addM2: true, addM3: false, hasNormalForce: true);
        var query = AddKineticFrictionForceSpecificDefaultSceneSettings(addM2: true, addM3: false);

        if (query.ObjectsFriction == null)
        {
            throw new ArgumentException("query.ObjectsFriction must not be null");
        }

        query.ObjectsFriction.Single().ObjectIsMoving = false;

        // Act
        parser.Parse(results, query);

        // Assert
        Assert.Multiple(() =>
        {
            var m1Obj = results.Single(x => x.GetId() == "m1");
            var m2Obj = results.Single(x => x.GetId() == "m2");

            Assert.That(m1Obj.Forces, Has.Exactly(5).Items);
            Assert.That(m2Obj.Forces, Has.Exactly(2).Items);

            Assert.That(m1Obj.Forces.OfType<KineticFrictionForce>(), Has.Exactly(0).Items);
            Assert.That(m2Obj.Forces.OfType<KineticFrictionForce>(), Has.Exactly(0).Items);
        });
    }

    [Test]
    public void TwoObjectsArePassed_M1MustHave1KineticFriction_M2ShouldNotHaveAny()
    {
        // Arrange
        var parser = new KineticFrictionForceParser();
        var results = AddKineticFrictionForceSpecificDefaultObjects(addM2: true, addM3: false, hasNormalForce: true);
        var query = AddKineticFrictionForceSpecificDefaultSceneSettings(addM2: true, addM3: false);

        // Act
        parser.Parse(results, query);

        // Assert
        Assert.Multiple(() =>
        {
            var m1Obj = results.Single(x => x.GetId() == "m1");
            var m2Obj = results.Single(x => x.GetId() == "m2");

            Assert.That(m1Obj.Forces, Has.Exactly(7).Items);
            Assert.That(m2Obj.Forces, Has.Exactly(2).Items);

            var m1KinFrForces = m1Obj.Forces.OfType<KineticFrictionForce>();
            Assert.That(m1KinFrForces, Has.Exactly(2).Items);
            Assert.That(m2Obj.Forces.OfType<KineticFrictionForce>(), Has.Exactly(0).Items);

            var m1KineticForce1 = m1KinFrForces.Where(x => x.Mass.Quantity == 10 && x.Coefficient.Quantity == 0.3
                && x.Angle == 180);
            Assert.That(m1KineticForce1, Has.Exactly(1).Items);
            Assert.That(m1KineticForce1.FirstOrDefault()?.Quantity, Is.EqualTo(29.4201).Within(0.0001));

            var m1KineticForce2 = m1KinFrForces.Where(x => x.Mass.Quantity == 5 && x.Coefficient.Quantity == 0.3
                && x.Angle == 180);
            Assert.That(m1KineticForce2, Has.Exactly(1).Items);
            Assert.That(m1KineticForce2.FirstOrDefault()?.Quantity, Is.EqualTo(14.71005).Within(0.0001));
        });
    }

    [Test]
    public void TwoObjectHaveKineticFriction_M1MustHave2KineticFrictions_M2MustHave1()
    {
        // Arrange
        var parser = new KineticFrictionForceParser();
        var results = AddKineticFrictionForceSpecificDefaultObjects(addM2: true, addM3: false, hasNormalForce: true);
        var query = AddKineticFrictionForceSpecificDefaultSceneSettings(addM2: true, addM3: false);

        query.ObjectsFriction?.Add(new FrictionSettings()
        {
            Angle = 180,
            TargetObj = "m1",
            SecondObj = "m2",
            Mu = 0.4,
            ObjectIsMoving = true
        });

        // Act
        parser.Parse(results, query);

        // Assert
        Assert.Multiple(() =>
        {
            var m1Obj = results.Single(x => x.GetId() == "m1");
            var m2Obj = results.Single(x => x.GetId() == "m2");

            Assert.That(m1Obj.Forces, Has.Exactly(8).Items);
            Assert.That(m2Obj.Forces, Has.Exactly(3).Items);

            var m1KinFrForces = m1Obj.Forces.OfType<KineticFrictionForce>();
            var m2KinFrForces = m2Obj.Forces.OfType<KineticFrictionForce>();

            Assert.That(m1KinFrForces, Has.Exactly(3).Items);
            Assert.That(m2KinFrForces, Has.Exactly(1).Items);

            var expectedK_grM1 = 0.3;
            var expectedK_M1M2 = 0.4;
            var expectedKinFAngle = 180;

            var m1KineticForce1 = m1KinFrForces.Where(x => x.Mass.Quantity == 10 && x.Coefficient.Quantity == expectedK_grM1
                && x.Angle == expectedKinFAngle);
            Assert.That(m1KineticForce1, Has.Exactly(1).Items);
            Assert.That(m1KineticForce1.FirstOrDefault()?.Quantity, Is.EqualTo(29.4201).Within(0.0001));

            var m1KineticForce2 = m1KinFrForces.Where(x => x.Mass.Quantity == 5 && x.Coefficient.Quantity == expectedK_grM1
                && x.Angle == expectedKinFAngle);
            Assert.That(m1KineticForce2, Has.Exactly(1).Items);
            Assert.That(m1KineticForce2.FirstOrDefault()?.Quantity, Is.EqualTo(14.71005).Within(0.0001));

            var m1KineticForce3 = m1KinFrForces.Where(x => x.Mass.Quantity == 5 && x.Coefficient.Quantity == expectedK_M1M2
                && x.Angle == expectedKinFAngle);
            Assert.That(m1KineticForce3, Has.Exactly(1).Items);
            Assert.That(m1KineticForce3.FirstOrDefault()?.Quantity, Is.EqualTo(19.6134).Within(0.0001));

            var m2KineticForce1 = m2KinFrForces.Where(x => x.Mass.Quantity == 5 && x.Coefficient.Quantity == expectedK_M1M2
                && x.Angle == expectedKinFAngle);
            Assert.That(m2KineticForce1, Has.Exactly(1).Items);
            Assert.That(m2KineticForce1.FirstOrDefault()?.Quantity, Is.EqualTo(19.6134).Within(0.0001));
        });
    }

    [TestCase(true)]
    [TestCase(false)]
    public void ThreeObjectsOnAScene_TwoOfThemHaveFriction_MustCorrectlyCalculateFrictionForces(bool swapObjFrictionOrder)
    {
        // Arrange
        var parser = new KineticFrictionForceParser();
        var results = AddKineticFrictionForceSpecificDefaultObjects(addM2: true, addM3: true, hasNormalForce: true);
        var query = AddKineticFrictionForceSpecificDefaultSceneSettings(addM2: true, addM3: true);

        query.ObjectsFriction?.Add(new FrictionSettings()
        {
            Angle = 180,
            TargetObj = "m1",
            SecondObj = "m2",
            Mu = 0.4,
            ObjectIsMoving = true
        });

        if (query.ObjectsFriction != null && swapObjFrictionOrder)
        {
            (query.ObjectsFriction[0].SecondObj, query.ObjectsFriction[0].TargetObj) =
                (query.ObjectsFriction[0].TargetObj, query.ObjectsFriction[0].SecondObj);

            (query.ObjectsFriction[1].SecondObj, query.ObjectsFriction[1].TargetObj) =
                (query.ObjectsFriction[1].TargetObj, query.ObjectsFriction[1].SecondObj);
        }

        // Act
        parser.Parse(results, query);

        // Assert
        Assert.Multiple(() =>
        {
            var m1Obj = results.Single(x => x.GetId() == "m1");
            var m2Obj = results.Single(x => x.GetId() == "m2");
            var m3Obj = results.Single(x => x.GetId() == "m3");

            Assert.That(m1Obj.Forces, Has.Exactly(12).Items);
            Assert.That(m2Obj.Forces, Has.Exactly(6).Items);
            Assert.That(m3Obj.Forces, Has.Exactly(2).Items);

            var m1KinFrForces = m1Obj.Forces.OfType<KineticFrictionForce>();
            var m2KinFrForces = m2Obj.Forces.OfType<KineticFrictionForce>();
            var m3KinFrForces = m3Obj.Forces.OfType<KineticFrictionForce>();

            Assert.That(m1KinFrForces, Has.Exactly(5).Items);
            Assert.That(m2KinFrForces, Has.Exactly(2).Items);
            Assert.That(m3KinFrForces, Has.Exactly(0).Items);

            var expectedK_grM1 = 0.3;
            var expectedK_M1M2 = 0.4;
            var expectedKinFAngle = 180;

            var m1KineticForce1 = m1KinFrForces.Where(x => x.Mass.Quantity == 10 && x.Coefficient.Quantity == expectedK_grM1
                && x.Angle == expectedKinFAngle);
            Assert.That(m1KineticForce1, Has.Exactly(1).Items);
            Assert.That(m1KineticForce1.FirstOrDefault()?.Quantity, Is.EqualTo(29.4201).Within(0.0001));

            var m1KineticForce2 = m1KinFrForces.Where(x => x.Mass.Quantity == 5 && x.Coefficient.Quantity == expectedK_grM1
                && x.Angle == expectedKinFAngle);
            Assert.That(m1KineticForce2, Has.Exactly(1).Items);
            Assert.That(m1KineticForce2.FirstOrDefault()?.Quantity, Is.EqualTo(14.71005).Within(0.0001));

            var m1KineticForce3 = m1KinFrForces.Where(x => x.Mass.Quantity == 7 && x.Coefficient.Quantity == expectedK_grM1
                && x.Angle == expectedKinFAngle);
            Assert.That(m1KineticForce3, Has.Exactly(1).Items);
            Assert.That(m1KineticForce3.FirstOrDefault()?.Quantity, Is.EqualTo(20.59407).Within(0.0001));

            var m1KineticForce4 = m1KinFrForces.Where(x => x.Mass.Quantity == 5 && x.Coefficient.Quantity == expectedK_M1M2
                && x.Angle == expectedKinFAngle);
            Assert.That(m1KineticForce4, Has.Exactly(1).Items);
            Assert.That(m1KineticForce4.FirstOrDefault()?.Quantity, Is.EqualTo(19.6134).Within(0.0001));

            var m1KineticForce5 = m1KinFrForces.Where(x => x.Mass.Quantity == 7 && x.Coefficient.Quantity == expectedK_M1M2
                && x.Angle == expectedKinFAngle);
            Assert.That(m1KineticForce5, Has.Exactly(1).Items);
            Assert.That(m1KineticForce5.FirstOrDefault()?.Quantity, Is.EqualTo(27.45876).Within(0.00001));

            var m2KineticForce1 = m2KinFrForces.Where(x => x.Mass.Quantity == 5 && x.Coefficient.Quantity == expectedK_M1M2
                && x.Angle == expectedKinFAngle);
            Assert.That(m2KineticForce1, Has.Exactly(1).Items);
            Assert.That(m2KineticForce1.FirstOrDefault()?.Quantity, Is.EqualTo(19.6134).Within(0.0001));

            var m2KineticForce2 = m2KinFrForces.Where(x => x.Mass.Quantity == 7 && x.Coefficient.Quantity == expectedK_M1M2
                && x.Angle == expectedKinFAngle);
            Assert.That(m2KineticForce2, Has.Exactly(1).Items);
            Assert.That(m2KineticForce2.FirstOrDefault()?.Quantity, Is.EqualTo(27.45876).Within(0.0001));
        });
    }

    [Test]
    public void ObjectIsSlidingOnAnAngledPlane_MustCalculateKineticFriction()
    {
        // Arrange
        var parser = new KineticFrictionForceParser();
        var results = PhysObjectHelpers.GetDefaultObjects(g: 10, angle: 30, addM2: false, addGround: true, addNormalForce: true);
        var query = PhysObjectHelpers.GetDefaultSceneSettings(g: 10, angle: 30, addM2: false, addGround: true);

        query.ObjectsFriction = [
            new FrictionSettings() { TargetObj = "m1", SecondObj = "ground", Mu = 0.3, Angle = 30, ObjectIsMoving = true }
        ];

        // Act
        parser.Parse(results, query);

        // Assert
        Assert.Multiple(() =>
        {
            var m1Obj = results.Single(x => x.GetId() == "m1");

            Assert.That(m1Obj.Forces, Has.Exactly(3).Items);

            var m1KinFrForces = m1Obj.Forces.OfType<KineticFrictionForce>().ToArray();

            Assert.That(m1KinFrForces, Has.Exactly(1).Items);

            var expectedK_grM1 = 0.3;
            var expectedKinFAngle = 30;
            var expectedKinForce = 25.98076;

            var m1KineticForce1 = m1KinFrForces.Where(x => x.Mass.Quantity == 10 && x.Coefficient.Quantity == expectedK_grM1
                && x.Angle == expectedKinFAngle);
            Assert.That(m1KineticForce1, Has.Exactly(1).Items);
            Assert.That(m1KineticForce1.FirstOrDefault()?.Quantity, Is.EqualTo(expectedKinForce).Within(0.0001));
        });
    }

    private static List<IPhysObject> AddKineticFrictionForceSpecificDefaultObjects(bool addM2, bool addM3, bool hasNormalForce)
    {
        var results = PhysObjectHelpers.GetDefaultObjects(g: null, angle: 0, addM2: addM2, addGround: true, addNormalForce: hasNormalForce);

        results[1].Forces.Add(new Force(10, 0, ForceType.Additional));

        if (addM3)
        {
            var m3ObjMass = new Mass(7);
            var acceleration = new Acceleration(Constants.Forces.g_Earth, 270);
            var m3WeightForce = new Force(m3ObjMass, acceleration, 0, ForceType.Weight);

            results.Add(new PointLikeParticle(m3ObjMass, [
                m3WeightForce,
                new Force(7 * Constants.Forces.g_Earth, 90, ForceType.Normal)
                {
                    Mass = m3ObjMass,
                    Acceleration = acceleration
                }
            ], "m3"));

            results[1].Forces.Add(m3WeightForce);
            results[1].Forces.Add(new Force(7 * Constants.Forces.g_Earth, 90, ForceType.Normal)
            {
                Mass = m3ObjMass,
                Acceleration = acceleration
            });

            results[2].Forces.Add(m3WeightForce);
            results[2].Forces.Add(new Force(7 * Constants.Forces.g_Earth, 90, ForceType.Normal)
            {
                Mass = m3ObjMass,
                Acceleration = acceleration
            });
        }

        return results;
    }

    private static SceneSettings AddKineticFrictionForceSpecificDefaultSceneSettings(bool addM2, bool addM3)
    {
        var query = PhysObjectHelpers.GetDefaultSceneSettings(g: null, angle: 0, addM2: addM2, addGround: true);

        query.Objects[0].Forces = [(new ForceSettings() { Quantity = 10, Angle = 0 })];

        query.ObjectsFriction = [
            new FrictionSettings() { TargetObj = "m1", SecondObj = "ground", Mu = 0.3, Angle = 180, ObjectIsMoving = true }
        ];

        if (addM3)
        {
            query.Objects.Add(new ObjectSettings()
            {
                Name = "m3",
                Mass = new QuantitySettings() { Quantity = 7, SiState = SIState.Known },
                Angle = 0,
                Forces = []
            });

            query.ObjectsPlacementOrder[0].Add("m3");
        }

        return query;
    }
}
