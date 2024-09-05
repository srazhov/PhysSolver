using System.Numerics;
using PhysHelper.Helpers;

namespace PhysHelper.SIObjects;

public class VectorQuantity
{
    private double _quantity;
    private double _angle;
    private double? _x;
    private double? _y;
    private double? _magnitude;

    public VectorQuantity(double quantity, double angle)
    {
        Quantity = quantity;
        Angle = angle;
    }

    public VectorQuantity(double x, double y, bool useComponents)
    {
        if (!useComponents)
        {
            throw new ArgumentException($"Cannot use this Constructor when {nameof(useComponents)} is set to false", nameof(useComponents));
        }

        Quantity = Math.Round(Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)), 5);
        var dotProduct = Vector2.Dot(new Vector2((float)x, (float)y), new Vector2(1, 0));
        Angle = Quantity == 0 ? 0 : Math.Round(Math.Acos(dotProduct / Quantity) * (180 / Math.PI), 2); ;
    }

    public double Quantity
    {
        get => _quantity;
        set
        {
            _quantity = Math.Abs(value);
            _x = null;
            _y = null;
            _magnitude = null;
        }
    }

    public double Angle
    {
        get => _angle;
        set
        {
            _angle = Math.Abs(value % 360);
            _x = null;
            _y = null;
            _magnitude = null;
        }
    }

    public double X => _x ??= Math.Round(Quantity * Math.Cos(HelperClass.GetAngleInRadians(Angle)), 5);

    public double Y => _y ??= Math.Round(Quantity * Math.Sin(HelperClass.GetAngleInRadians(Angle)), 5);

    public double Magnitude => _magnitude ??= Math.Round(Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2)), 5);
}
