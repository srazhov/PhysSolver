using PhysHelper.Enums;
using PhysHelper.Helpers;
using PhysHelper.SIObjects.Scalars;

namespace PhysHelper.SIObjects.Forces;

public class StaticForce : Force
{
    // TODO
    public StaticForce(Force weightForce, StaticFrictionCoefficient coefficient, double angle) : base(weightForce.Mass, weightForce.Acceleration, angle, ForceType.StaticFriction)
    {
        if (weightForce.ForceType != ForceType.Weight)
        {
            throw new ArgumentException("Provided force is not a Weight Force", nameof(weightForce));
        }

        Quantity = Math.Abs(Math.Round(weightForce.Quantity * Math.Cos(HelperClass.GetAngleInRadians(angle)), 5));
        WeightForce = weightForce;
        Coefficient = coefficient;
    }

    public Force WeightForce { get; set; }

    public StaticFrictionCoefficient Coefficient { get; set; }
}