using PhysHelper.Parsers.PhysObjectParsers;
using PhysHelper.Scenes.Objects;
using PhysHelper.Scenes.SceneSettings;

namespace PhysHelper.Tests.Parsers.PhysObjectParsers;

public class ObjectsCreationParserTests
{
    [Test]
    public void SceneSettingsIsPassed_MustReturnCorrectObjects()
    {
        // Arrange
        var query = new SceneSettings()
        {
            Global = null,
            Ground = new GroundSettings()
            {
                Angle = 0,
                Exists = true
            },
            Objects =
            [
                new(){
                    Name = "m1",
                    Mass = new MassSettings() {
                        Quantity = 10,
                        SiState = Enums.SIState.Known
                    },
                    Angle = 0,
                    Forces = []
                },
                new(){
                    Name = "m2",
                    Mass = new MassSettings() {
                        Quantity = 10,
                        SiState = Enums.SIState.Known
                    },
                    Angle = 0,
                    Forces = []
                }
            ],
            ObjectsPlacementOrder = [["ground", "m1", "m2"]],
            ObjectsFriction = null
        };

        var parser = new ObjectsCreationParser();
        var results = new List<IPhysObject>();

        // Act
        parser.Parse(results, query);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(results, Has.Exactly(3).Items);
            Assert.That(results.First(), Is.InstanceOf<Ground>());
            Assert.That(results.SingleOrDefault(x => x.GetId() == "m1"), Is.Not.Null);
            Assert.That(results.SingleOrDefault(x => x.GetId() == "m2"), Is.Not.Null);
        });
    }

    [Test]
    public void GroundSettingIsNotSpecified_MustNotReturnGroundObject()
    {
        // Arrange
        var query = new SceneSettings()
        {
            Global = null,
            Ground = null,
            Objects =
            [
                new(){
                    Name = "m1",
                    Mass = new MassSettings() {
                        Quantity = 10,
                        SiState = Enums.SIState.Known
                    },
                    Angle = 0,
                    Forces = []
                },
                new(){
                    Name = "m2",
                    Mass = new MassSettings() {
                        Quantity = 10,
                        SiState = Enums.SIState.Known
                    },
                    Angle = 0,
                    Forces = []
                }
            ],
            ObjectsPlacementOrder = [["m1", "m2"]],
            ObjectsFriction = null
        };

        var parser = new ObjectsCreationParser();
        var results = new List<IPhysObject>();

        // Act
        parser.Parse(results, query);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(results, Has.Exactly(2).Items);
            Assert.That(results, Has.No.TypeOf<Ground>());
            Assert.That(results.SingleOrDefault(x => x.GetId() == "m1"), Is.Not.Null);
            Assert.That(results.SingleOrDefault(x => x.GetId() == "m2"), Is.Not.Null);
        });
    }

    [Test]
    public void MassIsNotDefined_MustReturnSiState_Of_NeedsToBeKnown()
    {
        // Arrange
        var query = new SceneSettings()
        {
            Global = null,
            Ground = null,
            Objects =
            [
                new(){
                    Name = "m1",
                    Mass = new MassSettings() {
                        Quantity = 10,
                        SiState = Enums.SIState.NeedsToBeFound
                    },
                    Angle = 0,
                    Forces = []
                },
                new(){
                    Name = "m2",
                    Mass = new MassSettings() {
                        Quantity = 10,
                        SiState = Enums.SIState.Known
                    },
                    Angle = 0,
                    Forces = []
                }
            ],
            ObjectsPlacementOrder = [["m1", "m2"]],
            ObjectsFriction = null
        };

        var parser = new ObjectsCreationParser();
        var results = new List<IPhysObject>();

        // Act
        parser.Parse(results, query);

        // Assert
        var actualM1 = results.SingleOrDefault(x => x.GetId() == "m1");
        var actualM2 = results.SingleOrDefault(x => x.GetId() == "m2");
        Assert.Multiple(() =>
        {
            Assert.That(results, Has.Exactly(2).Items);
            Assert.That(results, Has.No.TypeOf<Ground>());

            Assert.That(actualM1, Is.Not.Null);
            Assert.That(actualM1?.Mass.SIState, Is.EqualTo(Enums.SIState.NeedsToBeFound));
            Assert.That(actualM1?.Mass.Quantity, Is.EqualTo(10));

            Assert.That(actualM2, Is.Not.Null);
            Assert.That(actualM2?.Mass.SIState, Is.EqualTo(Enums.SIState.Known));
            Assert.That(actualM2?.Mass.Quantity, Is.EqualTo(10));
        });
    }
}