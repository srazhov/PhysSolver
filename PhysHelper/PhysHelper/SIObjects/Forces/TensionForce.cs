using PhysHelper.Enums;

namespace PhysHelper.SIObjects.Forces;

public class TensionForce : Force
{
    public TensionForce(double quantity, double angle) : base(quantity, angle, ForceType.Tension)
    {
    }
}
