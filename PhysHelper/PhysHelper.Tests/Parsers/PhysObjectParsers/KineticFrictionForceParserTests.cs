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
    public void GroundIsPassed_MustNotAddAnyForcesToIt()
    {
        // Arrange
        var parser = new KineticFrictionForceParser();
        var results = AddKineticFrictionForceSpecificDefaultObjects();
        var query = AddKineticFrictionForceSpecificDefaultSceneSettings();

        results.RemoveAll(x => x.GetId() == "m2");
        results.Single(x => x.GetId() == "m1").Forces.RemoveAll(x => x.Mass.Quantity == 5);
        query.Objects.RemoveAll(x => x.Name == "m2");
        query.ObjectsPlacementOrder[0].RemoveAll(x => x == "m2");

        // Act
        parser.Parse(results, query);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(results, Has.Exactly(2).Items);

            var ground = results.OfType<Ground>().Single();
            var m1Obj = results.Single(x => x.GetId() == "m1");

            Assert.That(ground.Forces, Has.Exactly(0).Items);
            Assert.That(m1Obj.Forces, Has.Exactly(4).Items);

            var m1KineticFrictionForces = m1Obj.Forces.OfType<KineticFrictionForce>();
            Assert.That(m1Obj.Forces.OfType<KineticFrictionForce>(), Has.Exactly(1).Items);
        });
    }

    [Test]
    public void ObjectHasNoNormalForce_MustSkipIt()
    {
        // Arrange
        var parser = new KineticFrictionForceParser();
        var results = AddKineticFrictionForceSpecificDefaultObjects();
        var query = AddKineticFrictionForceSpecificDefaultSceneSettings();

        results.RemoveAll(x => x.GetId() == "m2");
        results.Single(x => x.GetId() == "m1").Forces.RemoveAll(x => x.Mass.Quantity == 5 || x.ForceType == ForceType.Normal);
        query.Objects.RemoveAll(x => x.Name == "m2");
        query.ObjectsPlacementOrder[0].RemoveAll(x => x == "m2");

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
    public void TwoObjectsArePassed_M1MustHave1KineticFriction_M2ShouldNotHaveAny()
    {
        // Arrange
        var parser = new KineticFrictionForceParser();
        var results = AddKineticFrictionForceSpecificDefaultObjects();
        var query = AddKineticFrictionForceSpecificDefaultSceneSettings();

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
        var results = AddKineticFrictionForceSpecificDefaultObjects();
        var query = AddKineticFrictionForceSpecificDefaultSceneSettings();

        query.ObjectsFriction?.Add(new KineticFrictionSettings()
        {
            Angle = 180,
            FirstObj = "m1",
            SecondObj = "m2",
            Mu = 0.4
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
        var results = AddKineticFrictionForceSpecificDefaultObjects(true);
        var query = AddKineticFrictionForceSpecificDefaultSceneSettings(true);

        query.ObjectsFriction?.Add(new KineticFrictionSettings()
        {
            Angle = 180,
            FirstObj = "m1",
            SecondObj = "m2",
            Mu = 0.4
        });

        if (query.ObjectsFriction != null && swapObjFrictionOrder)
        {
            (query.ObjectsFriction[0].SecondObj, query.ObjectsFriction[0].FirstObj) =
                (query.ObjectsFriction[0].FirstObj, query.ObjectsFriction[0].SecondObj);

            (query.ObjectsFriction[1].SecondObj, query.ObjectsFriction[1].FirstObj) =
                (query.ObjectsFriction[1].FirstObj, query.ObjectsFriction[1].SecondObj);
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

    private static List<IPhysObject> AddKineticFrictionForceSpecificDefaultObjects(bool addThirdObj = false)
    {
        var results = PhysObjectHelpers.GetDefaultObjects();

        results[1].Forces.Add(new Force(results[1].Forces[0].Mass, results[1].Forces[0].Acceleration, 90, ForceType.Normal));
        results[1].Forces.Add(new Force(results[1].Forces[1].Mass, results[1].Forces[1].Acceleration, 90, ForceType.Normal));
        results[1].Forces.Add(new Force(10, 0, ForceType.Additional));

        results[2].Forces.Add(new Force(results[2].Forces[0].Mass, results[2].Forces[0].Acceleration, 90, ForceType.Normal));

        if (addThirdObj)
        {
            var m3ObjMass = new Mass(7);
            results.Add(new PointLikeParticle(m3ObjMass, [
                new Force(m3ObjMass, new Acceleration(Constants.Forces.g_Earth, 270), 0, ForceType.Weight),
                new Force(m3ObjMass, new Acceleration(Constants.Forces.g_Earth, 90), 0, ForceType.Normal)
            ], "m3"));

            results[1].Forces.Add(new Force(m3ObjMass, new Acceleration(Constants.Forces.g_Earth, 270), 270, ForceType.Weight));
            results[1].Forces.Add(new Force(m3ObjMass, new Acceleration(Constants.Forces.g_Earth, 90), 90, ForceType.Normal));

            results[2].Forces.Add(new Force(m3ObjMass, new Acceleration(Constants.Forces.g_Earth, 270), 270, ForceType.Weight));
            results[2].Forces.Add(new Force(m3ObjMass, new Acceleration(Constants.Forces.g_Earth, 90), 90, ForceType.Normal));
        }

        return results;
    }

    private static SceneSettings AddKineticFrictionForceSpecificDefaultSceneSettings(bool addThirdObj = false)
    {
        var query = PhysObjectHelpers.GetDefaultSceneSettings();

        query.Objects[0].Forces = [(new ForceSettings() { Quantity = 10, Angle = 0 })];

        query.ObjectsFriction = [
            new KineticFrictionSettings() { FirstObj = "m1", SecondObj = "ground", Mu = 0.3, Angle = 180 }
        ];

        if (addThirdObj)
        {
            query.Objects.Add(new ObjectSettings()
            {
                Name = "m3",
                Mass = new MassSettings() { Quantity = 7, SiState = SIState.Known },
                Angle = 0,
                Forces = []
            });

            query.ObjectsPlacementOrder[0].Add("m3");
        }

        return query;
    }
}
