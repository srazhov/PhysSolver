namespace PhysHelper.Scenes.SceneSettings;

public class SceneSettings
{
    public GlobalSceneSettings? Global { get; set; }

    public GroundSettings? Ground { get; set; }

    public required List<ObjectSettings> Objects { get; set; }
}

public class GlobalSceneSettings
{
    public double G { get; set; }
}

public class GroundSettings
{
    public bool Exists { get; set; }

    public double Angle { get; set; }

    public List<string>? ObjectsOnTop { get; set; }
}
