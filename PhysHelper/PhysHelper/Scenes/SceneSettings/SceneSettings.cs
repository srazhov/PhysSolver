namespace PhysHelper.Scenes.SceneSettings;

public class SceneSettings
{
    public GlobalSceneSettings? Global { get; set; }

    public GroundSettings? Ground { get; set; }

    public required List<ObjectSettings> Objects { get; set; }

    public required List<List<string>> ObjectsPlacementOrder { get; set; }

    public List<FrictionSettings>? ObjectsFriction { get; set; }

    public List<TensionSettings>? Tensions { get; set; }
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

public class FrictionSettings
{
    public required string TargetObj { get; set; }

    public required string SecondObj { get; set; }

    public required double Mu { get; set; }

    public required double Angle { get; set; }

    public required bool ObjectIsMoving { get; set; }
}

public class TensionSettings
{
    public required string TargetObj { get; set; }

    public required string SecondObj { get; set; }

    public required double TargetObjAngle { get; set; }

    public required double SecondObjAngle { get; set; }
}
