using PhysHelper.Scenes;
using PhysHelper.Scenes.Objects;
using PhysHelper.Scenes.SceneSettings;

namespace PhysHelper.Tests.Scenes
{
    public class SceneTests
    {
        [Test]
        public void EmptySceneMustHaveOnlyTheGroundAsAnObject()
        {
            // Arrange
            var settings = new SceneSettings()
            {
                Ground = new GroundSettings()
                {
                    Exists = true,
                    Angle = 30,

                },
                Objects = [],
                ObjectsPlacementOrder = [],
                ObjectsFriction = null,
                Global = null
            };

            IScene scene = new Scene(settings);

            // Act
            var objs = scene.GetAllObjects();

            // Assert 
            Assert.Multiple(() =>
            {
                Assert.That(objs, Has.Length.EqualTo(1));
                Assert.That(objs.Single(), Is.InstanceOf<Ground>());
            });
        }
    }
}

