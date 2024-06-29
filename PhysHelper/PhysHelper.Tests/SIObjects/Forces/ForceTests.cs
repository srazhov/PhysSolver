using PhysHelper.Factories;
using PhysHelper.SIObjects;
using PhysHelper.SIObjects.Forces;

namespace PhysHelper.Tests.SIObjects.Forces
{
    public class ForceTests
    {
        [Test]
        public void MustReturnCorrectUnitOfMeasurement()
        {
            // Arrange
            var f = new Force(1, 1, 0);

            // Act
            var actual = f.UnitOfMeasure;

            // Assert
            var expected = Constants.Forces.Newton;
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void MassOf100KgIsGiven_MustReturnCorrectMagnitudeAmountInNewtons()
        {
            // Arrange
            var f = ForceFactory.GetWeightForce(100, 270);

            // Act
            var actual = f.Magnitude;

            // Assert
            var expected = 980.67;
            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase(100, 30, 980.67)]
        [TestCase(1, 45, 9.8067)]
        [TestCase(15.2, 123.5, 149.06184)]
        public void MassAtAnAngleIsGiven_MustReturnCorrectMagnitude(double mass, double angle, double expected)
        {
            // Arrange
            var f = ForceFactory.GetWeightForce(mass, angle);

            // Act
            var actual = f.Magnitude;

            // Assert
            Assert.That(actual, Is.EqualTo(expected).Within(0.00002));
        }

        [TestCase(100, 270, 0, -980.67)]
        [TestCase(100, 30, 849.28512, 490.335)]
        [TestCase(1, 136.72, -7.1394, 6.72312)]
        [TestCase(18.53, 72.1, 55.85228, 172.92198)]
        public void MassAtAnAngleIsGiven_MustReturnCorrectMagnitudesForXandY(double mass, double angle, double expectedX, double expectedY)
        {
            // Arrange
            var f = ForceFactory.GetWeightForce(mass, angle);

            // Act
            var actual = f.Direction;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actual.X, Is.EqualTo(expectedX).Within(0.00002));
                Assert.That(actual.Y, Is.EqualTo(expectedY).Within(0.00002));
            });
        }
    }
}

