using PhysHelper.Scenes;
using PhysHelper.Scenes.Objects;
using PhysHelper.SIObjects.Forces;

namespace PhysHelper.Tests.Scenes
{
    public class SceneTests
    {
        [Test]
        public void EmptySceneMustHaveOnlyTheGroundAsAnObject()
        {
            // Arrange
            IScene scene = new Scene();

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

