namespace PhysHelper.Scenes.SceneSettings;

public class SceneSettings
{
    public GlobalSceneSettings? Global { get; set; }

    public GroundSettings? Ground { get; set; }

    public required List<ObjectSettings> Objects { get; set; }

    public required List<List<string>> ObjectsPlacementOrder { get; set; }

    public List<KineticFrictionSettings>? ObjectsFriction { get; set; }
}

public class GlobalSceneSettings
{
    public double G { get; set; }
}

public class GroundSettings
{
    public bool Exists { get; set; }

    public double Angle { get; set; }
}

public class KineticFrictionSettings
{
    public required string FirstObj { get; set; }

    public required string SecondObj { get; set; }

    public required double K { get; set; }

    public required double Angle { get; set; }
}
