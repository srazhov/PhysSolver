using PhysHelper.SIObjects.Forces;

namespace PhysHelper.Tests.SIObjects.Forces
{
    public class KineticFrictionForceTests
    {
        [Test]
        public void BodyMovesOnARoughPlane_MustReturnKineticFrictionForce()
        {
            // Arrange
            var kF = new KineticFrictionForce(0.2, 100, -180);

            // Act
            var actualX = kF.Direction.X;
            var actualY = kF.Direction.Y;

            // Assert
            var expectedX = -196.134;
            var expectedY = 0;
            Assert.Multiple(() =>
            {
                Assert.That(actualX, Is.EqualTo(expectedX));
                Assert.That(actualY, Is.EqualTo(expectedY));
            });
        }
    }
}

