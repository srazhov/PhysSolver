using PhysHelper.Factories;
using PhysHelper.Scenes.ObjectRelations;
using PhysHelper.Scenes.Objects;

namespace PhysHelper.Tests.Scenes.ObjectRelations
{
    public class ObjectRelationsTests
    {
        [Test]
        public void TwoObjectsWithFrictionAreGiven_MustConnectTheTwoAndShowTheKineticFriction()
        {
            // Arrange
            var expectedObj2Mass = 2;
            var expectedkineticFC = 0.2;
            var obj2 = PointLikeParticleFactory.GetParticle(2);

            // Act
            var relation = new ObjectRelation().IsOnTopOf(obj2).HaveKineticFriction(0.2);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(relation.BottomObject, Is.Not.Null);
                Assert.That(relation.BottomObject?.Mass, Is.EqualTo(expectedObj2Mass));

                Assert.That(relation.KineticCoefficient, Is.EqualTo(expectedkineticFC));
            });
        }

        [Test]
        public void ObjectLiesOnAFrictionlessGround_KineticFrictionMustBe0()
        {
            // Arrange
            var expectedObj2Mass = 0;
            var expectedkineticFC = 0;

            // Act
            var relation = new ObjectRelation().IsOnTheGround();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(relation.BottomObject, Is.Not.Null);
                Assert.That(relation.BottomObject?.Mass, Is.EqualTo(expectedObj2Mass));
                Assert.That(relation.BottomObject, Is.InstanceOf<Ground>());

                Assert.That(relation.KineticCoefficient, Is.EqualTo(expectedkineticFC));
            });
        }

        [Test]
        public void ObjectLiesOnAGroundWithFriction_KineticFrictionMustBePresent()
        {
            // Arrange
            var expectedObj2Mass = 0;
            var expectedkineticFC = 0.5;

            // Act
            var relation = new ObjectRelation().IsOnTheGround().HaveKineticFriction(0.5);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(relation.BottomObject, Is.Not.Null);
                Assert.That(relation.BottomObject?.Mass, Is.EqualTo(expectedObj2Mass));
                Assert.That(relation.BottomObject, Is.InstanceOf<Ground>());

                Assert.That(relation.KineticCoefficient, Is.EqualTo(expectedkineticFC));
            });
        }

        [Test]
        public void ObjectLiesOnAGroundWithFrictionAndConstantForceIsApplied_KineticFrictionAndTheForceMustBePresent()
        {
            // Arrange
            var expectedObj2Mass = 0;
            var expectedkineticFC = 0.5;
            var expectedForce = ForceFactory.GetConstantForce(10, 0);

            // Act
            var relation = new ObjectRelation().IsOnTheGround().HaveKineticFriction(0.5).HasConstantForce(expectedForce);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(relation.BottomObject, Is.Not.Null);
                Assert.That(relation.BottomObject?.Mass, Is.EqualTo(expectedObj2Mass));
                Assert.That(relation.BottomObject, Is.InstanceOf<Ground>());

                Assert.That(relation.KineticCoefficient, Is.EqualTo(expectedkineticFC));
                Assert.That(relation.MovingForce, Is.EqualTo(expectedForce));
            });
        }


        [Test]
        public void GivenFrictionButSecondObjectIsNotSpecified_MustThrowAnArgumentException()
        {
            // Act, Assert
            Assert.Throws<ArgumentException>(() => { new ObjectRelation().HaveKineticFriction(0.2); });
        }
    }
}
