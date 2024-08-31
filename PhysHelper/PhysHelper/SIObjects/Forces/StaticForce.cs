using PhysHelper.Enums;
using PhysHelper.Helpers;
using PhysHelper.SIObjects.Scalars;

namespace PhysHelper.SIObjects.Forces;

public class StaticForce : Force
{
    // TODO
    public StaticForce(Force normalForce, StaticFrictionCoefficient coefficient, double angle) : base(normalForce.Mass, normalForce.Acceleration, angle, ForceType.StaticFriction)
    {
        if (normalForce.ForceType != ForceType.Normal)
        {
            throw new ArgumentException("Provided force is not a Normal Force", nameof(normalForce));
        }

        Quantity = Math.Abs(Math.Round(normalForce.Quantity * Math.Cos(HelperClass.GetAngleInRadians(angle)), 5));
        this.NormalForce = normalForce;
        Coefficient = coefficient;
    }

    public Force NormalForce { get; set; }

    public StaticFrictionCoefficient Coefficient { get; set; }
}