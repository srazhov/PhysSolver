using System.ComponentModel.DataAnnotations;
using PhysHelper.Enums;

namespace PhysHelper.Scenes.SceneSettings;

public class ObjectSettings
{
    [Required]
    public required string Name { get; set; }

    public double Angle { get; set; }

    [Required]
    public required QuantitySettings Mass { get; set; }

    public ElasticForceSettings? ElasticForce { get; set; }

    public List<ForceSettings>? Forces { get; set; }
}

public class ElasticForceSettings
{
    public double RestingLength { get; set; }

    public double X { get; set; }

    public double K { get; set; }

    public double Angle { get; set; }
}

public class ForceSettings
{
    public double Quantity { get; set; }

    public double Angle { get; set; }

    public SIState SiState { get; set; }

    public QuantitySettings? Acceleration { get; set; }

    public bool IsNetForce { get; set; }
}

public class QuantitySettings
{
    public double Quantity { get; set; }

    public SIState SiState { get; set; }
}
