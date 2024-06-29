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

        public static Force GetConstantForce(double force, double angle)
        {
            var f = new Force(force, angle);

            return f;
        }

        public static void CreateKineticFrictionForceBetweenObjects(IPhysObject objectX, IPhysObject objectY, double k, double massOfObjOnTop, double angle)
        {
            var kF_Y = new KineticFrictionForce(objectY.GetId(), k, massOfObjOnTop, angle);
            objectX.AddForce(kF_Y);

            var kF_X = new KineticFrictionForce(objectX.GetId(), k, massOfObjOnTop, angle);
            objectY.AddForce(kF_X);
        }
    }
}
