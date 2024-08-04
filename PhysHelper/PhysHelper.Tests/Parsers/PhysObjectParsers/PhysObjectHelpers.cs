using PhysHelper.Scenes.Objects;
using PhysHelper.Scenes.SceneSettings;
using PhysHelper.SIObjects;
using PhysHelper.SIObjects.Forces;
using PhysHelper.SIObjects.Kinematics;
using PhysHelper.SIObjects.Scalars;

namespace PhysHelper.Tests.Parsers.PhysObjectParsers;

public static class PhysObjectHelpers
{
    public static List<IPhysObject> GetDefaultObjects()
    {
        var g = new Acceleration(Constants.Forces.g_Earth, 270);
        return [
            new Ground(),
            new PointLikeParticle(new Mass(10), [
                new Force(new Mass(10), g, 270, Enums.ForceType.Weight),
                new Force(new Mass(5), g, 270, Enums.ForceType.Weight)
            ], "m1"),
            new PointLikeParticle(new Mass(5), [
                new Force(new Mass(5), g, 270, Enums.ForceType.Weight)
            ], "m2")
        ];
    }

    public static SceneSettings GetDefaultSceneSettings()
    {
        return new SceneSettings()
        {
            Global = new GlobalSceneSettings() { },
            Ground = new GroundSettings()
            {
                Exists = true,
                Angle = 0
            },
            Objects = [
                new ObjectSettings(){
                    Name = "m1",
                    Mass = new MassSettings() { Quantity = 10, SiState = Enums.SIState.Known },
                    Angle = 0,
                    Forces = null
                },
                new ObjectSettings(){
                    Name = "m2",
                    Mass = new MassSettings() { Quantity = 5, SiState = Enums.SIState.Known },
                    Angle = 0,
                    Forces = null,
                }
            ],
            ObjectsPlacementOrder = [
                ["ground", "m1", "m2"]
            ],
            ObjectsFriction = null
        };
    }

}