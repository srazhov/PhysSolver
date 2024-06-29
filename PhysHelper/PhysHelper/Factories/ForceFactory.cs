using PhysHelper.Scenes.Objects;
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

        public static void CreateKineticFrictionForceBetweenObjects(IPhysObject objectX, IPhysObject objectY, double k, double mass, double angle)
        {
            var kF_Y = new KineticFrictionForce(objectY.GetId(), k, mass, angle);
            objectX.AddForce(kF_Y);

            var kF_X = new KineticFrictionForce(objectX.GetId(), k, mass, angle);
            objectY.AddForce(kF_X);
        }
    }
}

