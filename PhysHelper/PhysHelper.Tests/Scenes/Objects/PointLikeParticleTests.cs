using System;
using PhysHelper.Factories;

namespace PhysHelper.Tests.Scenes
{
    public class PointLikeParticleTests
    {
        [Test]
        public void ParticleWithMass100KgGiven_MustReturnCorrectMagnitudesOfForces()
        {
            // Arrange
            var expected = new[] { 980.67, 980.67 };

            // Act
            var m = PointLikeParticleFactory.GetParticle(100);
            var forces = m.GetAllForces();

            // Assert
            for (int i = 0; i < forces.Count; i++)
            {
                Assert.That(forces[i].Magnitude, Is.EqualTo(expected[i]));
            }
        }
    }
}

