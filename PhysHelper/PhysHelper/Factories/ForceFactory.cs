using PhysHelper.SIObjects;
using PhysHelper.SIObjects.Forces;

namespace PhysHelper.Factories
{
    public static class ForceFactory
    {
        public static Force GetWeightForce(double mass, double angle = 270)
        {
            var wF = new Force(mass, Constants.Forces.g_Earth, angle);
            return wF;
        }

        public static Force GetNormalForce(double mass)
        {
            var nF = new Force(mass, Constants.Forces.g_Earth, 90);
            return nF;
        }
    }
}

