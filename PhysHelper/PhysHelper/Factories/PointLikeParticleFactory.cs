using PhysHelper.Scenes.Objects;

namespace PhysHelper.Factories
{
	public static class PointLikeParticleFactory
	{
		public static IPhysObject GetParticle(double mass)
		{
			return new PointLikeParticle(mass);
		}
	}
}

