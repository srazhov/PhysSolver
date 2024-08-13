using PhysHelper.Enums;
using PhysHelper.SIObjects.Scalars;

namespace PhysHelper.SIObjects.Forces;

public class ElasticForce(double k, double x, double restingLength, double angle) : Force(k * x, angle, ForceType.Elastic)
{
    public SpringConstant K { get; set; } = new SpringConstant(k);

    public SpringExtension X { get; set; } = new SpringExtension(x);

    public SpringRestingLength Length { get; set; } = new SpringRestingLength(restingLength);
}
