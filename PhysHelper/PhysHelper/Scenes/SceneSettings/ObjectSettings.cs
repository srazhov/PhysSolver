using System.ComponentModel.DataAnnotations;
using PhysHelper.Enums;

namespace PhysHelper.Scenes.SceneSettings;

public class ObjectSettings
{
    [Required]
    public required string Name { get; set; }

    [Required]
    public required MassSettings Mass { get; set; }

    public double Angle { get; set; }

    public List<ForceSettings>? Forces { get; set; }
}

public class ForceSettings
{
    public double Quantity { get; set; }

    public double Angle { get; set; }
}

public class MassSettings
{
    public double Quantity { get; set; }

    public SIState SiState { get; set; }
}
