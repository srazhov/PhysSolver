using PhysHelper.Enums;
using PhysHelper.SIObjects;
using PhysHelper.SIObjects.Kinematics;

namespace PhysHelper.Tests.SIObjects
{
    public class VectorTests
    {
        [TestCase(360, ExpectedResult = 0)]
        [TestCase(720, ExpectedResult = 0)]
        [TestCase(361, ExpectedResult = 1)]
        [TestCase(36, ExpectedResult = 36)]
        [TestCase(520, ExpectedResult = 160)]
        [TestCase(-520, ExpectedResult = 160)]
        public double RandomAngleIsGiven_MustReturnCorrectAngleUpTo360(double angle)
        {
            // Arrange
            Vector vector = new Acceleration(1, angle);

            // Act
            var actual = vector.Angle;

            // Assert 
            return actual;
        }

        [TestCase(100, 30, 100)]
        [TestCase(1, 45, 1)]
        [TestCase(15.2, 123.5, 15.2)]
        public void VectorIsGiven_MustReturnCorrectMagnitude(double newtons, double angle, double expected)
        {
            // Arrange
            Vector acc = new Acceleration(newtons, angle);

            // Act
            var actual = acc.Magnitude;

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Within(0.00002));
        }

        [TestCase(100, 270, 0, -100)]
        [TestCase(100, 30, 86.60254, 50)]
        [TestCase(1, 136.72, -0.72801, 0.68556)]
        [TestCase(18.53, 72.1, 5.69531, 17.63304)]
        public void VectorIsGiven_MustReturnCorrectXandYcomponents(double quantity, double angle, double expectedX, double expectedY)
        {
            // Arrange
            Vector acc = new Acceleration(quantity, angle);

            // Act
            var actualX = acc.Direction.X;
            var actualY = acc.Direction.Y;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actualX, Is.EqualTo(expectedX).Within(0.00002));
                Assert.That(actualY, Is.EqualTo(expectedY).Within(0.00002));
            });
        }

        [Test]
        public void DefaultConstructorIsUsed_MustReturnSIStateEqualsToUnimportant()
        {
            // Arrange
            Vector acc = new Acceleration();

            // Act
            var actualSiState = acc.SIState;
            var actualQuantity = acc.Quantity;
            var actualAngle = acc.Angle;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actualSiState, Is.EqualTo(SIState.Unimportant));
                Assert.That(actualQuantity, Is.NaN);
                Assert.That(actualAngle, Is.EqualTo(0));
            });
        }
    }
}

