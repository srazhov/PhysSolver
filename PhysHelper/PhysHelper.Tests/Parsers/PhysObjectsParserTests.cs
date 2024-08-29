using PhysHelper.Enums;
using PhysHelper.Parsers;
using PhysHelper.SIObjects;
using PhysHelper.Tests.Parsers.PhysObjectParsers;

namespace PhysHelper.Tests.Parsers;

public class PhysObjectsParserTests
{
    [Test]
    public void QueryWithTwoDefinedObjectsSittingOnAFlatPlaneIsPassed_MustParse2ObjectsForces()
    {
        // Arrange
        var query = PhysObjectHelpers.GetDefaultSceneSettings(g: 9.8, angle: 0, addM2: true, addGround: true);

        // Act
        var results = PhysObjectsParser.Parse(query);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(results, Has.Exactly(3).Items);

            // Assert there is a Ground object
            var ground = results.SingleOrDefault(x => x.GetId() == Constants.GroundId);
            Assert.That(ground, Is.Not.Null);
            Assert.That(ground?.Forces, Has.Exactly(0).Items);

            // Assert m1 Forces
            var m1 = results.SingleOrDefault(x => x.GetId() == "m1");
            Assert.That(m1, Is.Not.Null);
            Assert.That(m1?.Forces, Has.Exactly(5).Items);
            Assert.That(m1?.Forces.Where(x => x.ForceType == ForceType.Weight), Has.Exactly(2).Items);
            Assert.That(m1?.Forces.Where(x => x.ForceType == ForceType.Normal), Has.Exactly(2).Items);
            Assert.That(m1?.Forces.Where(x => x.ForceType == ForceType.Net), Has.Exactly(1).Items);

            // Assert m2 Forces
            var m2 = results.SingleOrDefault(x => x.GetId() == "m2");
            Assert.That(m2, Is.Not.Null);
            Assert.That(m2?.Forces, Has.Exactly(3).Items);
            Assert.That(m2?.Forces.Where(x => x.ForceType == ForceType.Weight), Has.Exactly(1).Items);
            Assert.That(m2?.Forces.Where(x => x.ForceType == ForceType.Normal), Has.Exactly(1).Items);
            Assert.That(m2?.Forces.Where(x => x.ForceType == ForceType.Net), Has.Exactly(1).Items);
        });
    }
}