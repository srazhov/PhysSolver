using Moq;
using PhysHelper.Enums;
using PhysHelper.SIObjects;
using PhysHelper.SIObjects.Forces;
using PhysHelper.SIObjects.Kinematics;
using PhysHelper.SIObjects.Scalars;

namespace PhysHelper.Tests.SIObjects.Forces
{
    public class ForceTests
    {
        [Test]
        public void MustReturnCorrectUnitOfMeasurement()
        {
            // Arrange
            var mockAcceleration = new Mock<Acceleration>(MockBehavior.Strict);
            var mockMass = new Mock<Mass>(MockBehavior.Strict);

            var force = new Force(mockMass.Object, mockAcceleration.Object, 0);

            // Act
            var actual = force.UnitOfMeasure;

            // Assert
            var expected = Constants.Forces.Newton;
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void MassOf100KgIsGiven_MustReturnCorrectMagnitudeAmountInNewtons()
        {
            // Arrange
            var angle = 270;
            var mass = 100;

            var mockAcceleration = new Mock<Acceleration>(MockBehavior.Strict, Constants.Forces.g_Earth, angle);
            var mockMass = new Mock<Mass>(MockBehavior.Strict, mass);
            var f = new Force(mockMass.Object, mockAcceleration.Object, angle);

            // Act
            var actual = f.Magnitude;

            // Assert
            var expected = 980.67;
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ConstructorThatOnlySpecifiesQuantityIsUsed_MustReturnSIStateOfKnown_AccelerationShouldBeUndefined()
        {
            // Arrange
            var newtons = 1;
            var angle = 270;
            var v = new Force(newtons, angle);

            // Act 
            var actualSiState = v.SIState;
            var actualAcceleration = v.Acceleration;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actualSiState, Is.EqualTo(SIState.Known));

                Assert.That(actualAcceleration, Is.Not.Null);
                Assert.That(actualAcceleration.SIState, Is.EqualTo(SIState.Unimportant));
                Assert.That(actualAcceleration.Quantity, Is.EqualTo(0));
                Assert.That(actualAcceleration.Angle, Is.EqualTo(0));
            });
        }

        [Test]
        public void ConstructorThatOnlySpecifiesQuantityIsUsed_MustReturnSIStateOfKnown_MassShouldBeUndefined()
        {
            // Arrange
            var newtons = 1;
            var angle = 270;
            var v = new Force(newtons, angle);

            // Act 
            var actualSiState = v.SIState;
            var actualMass = v.Mass;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actualSiState, Is.EqualTo(SIState.Known));

                Assert.That(actualMass, Is.Not.Null);
                Assert.That(actualMass.SIState, Is.EqualTo(SIState.Unimportant));
                Assert.That(actualMass.Quantity, Is.EqualTo(0));
            });
        }
    }
}

