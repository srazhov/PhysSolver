using PhysHelper.Enums;
using PhysHelper.Helpers;

namespace PhysHelper.SIObjects.Forces;

public class TensionForce : Force
{
    public TensionForce(double x, double angle) : base(0, angle, ForceType.Tension)
    {
        Quantity = -1 * Math.Round(x / Math.Cos(HelperClass.GetAngleInRadians(angle)), 5);
    }
}