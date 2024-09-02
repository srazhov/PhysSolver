using PhysHelper.Enums;
using PhysHelper.Helpers;
using PhysHelper.Scenes.Objects;
using PhysHelper.Scenes.SceneSettings;
using PhysHelper.SIObjects;
using PhysHelper.SIObjects.Forces;
using PhysHelper.SIObjects.Kinematics;
using PhysHelper.SIObjects.Scalars;

namespace PhysHelper.Tests.Parsers;

public static class PhysObjectHelpers
{
    public static List<IPhysObject> GetDefaultObjects(double? g, double angle, bool addM2, bool addGround, bool addNormalForce)
    {
        var grav = new Acceleration(g ?? Constants.Forces.g_Earth, 270);
        var m1Mass = new Mass(10);
        var results = new List<IPhysObject>()
        {
            new PointLikeParticle(m1Mass, [
                new Force(m1Mass, grav, 270, ForceType.Weight)
            ], "m1"),
        };

        if (addGround)
        {
            results.Insert(0, new Ground());
        }

        var m1 = results.Single(x => x.GetId() == "m1");
        if (addM2)
        {
            var m2Mass = new Mass(5);
            var m2 = new PointLikeParticle(m2Mass, [
                new Force(m2Mass, grav, 270, ForceType.Weight)
            ], "m2");

            results.Add(m2);
            m1.Forces.Add(m2.Forces.Single()); // add m2's weight to m1
        }

        if (addNormalForce)
        {
            var m1NormalForce = Math.Round(grav.Quantity * 10 * Math.Sin(HelperClass.GetAngleInRadians(angle + 90)), 5);
            if (addGround)
            {
                m1.Forces.Add(new Force(m1NormalForce, angle + 90, ForceType.Normal)
                {
                    Mass = m1.Forces[0].Mass,
                    Acceleration = m1.Forces[0].Acceleration
                });
            }

            if (addM2)
            {
                var m2 = results.Single(x => x.GetId() == "m2");
                var m2NormalForce = Math.Round(grav.Quantity * 5 * Math.Sin(HelperClass.GetAngleInRadians(angle + 90)), 5);
                m1.Forces.Add(new Force(m2NormalForce, angle + 90, ForceType.Normal)
                {
                    Mass = m2.Forces[0].Mass,
                    Acceleration = m2.Forces[0].Acceleration
                });

                m2.Forces.Add(new Force(m2NormalForce, angle + 90, ForceType.Normal)
                {
                    Mass = m2.Forces[0].Mass,
                    Acceleration = m2.Forces[0].Acceleration
                });
            }
        }

        return results;
    }

    public static SceneSettings GetDefaultSceneSettings(double? g, double angle, bool addM2, bool addGround)
    {
        var query = new SceneSettings()
        {
            Global = new GlobalSceneSettings() { G = g ?? Constants.Forces.g_Earth },
            Ground = null,
            Objects = [
                new ObjectSettings(){
                    Name = "m1",
                    Mass = new QuantitySettings() { Quantity = 10, SiState = SIState.Known },
                    Angle = angle,
                    Forces = null,
                    ElasticForce = null
                }
            ],
            ObjectsPlacementOrder = [
                ["m1"]
            ],
            ObjectsFriction = null
        };

        if (addGround)
        {
            query.Ground = new GroundSettings()
            {
                Exists = true,
                Angle = angle
            };

            query.ObjectsPlacementOrder[0].Insert(0, "ground");
        }

        if (addM2)
        {
            query.Objects.Add(new ObjectSettings()
            {
                Name = "m2",
                Mass = new QuantitySettings() { Quantity = 5, SiState = SIState.Known },
                Angle = angle,
                Forces = null,
                ElasticForce = null
            });

            query.ObjectsPlacementOrder[0].Add("m2");
        }

        return query;
    }

    public static Force? AssertOneForce(IEnumerable<Force>? forces, ForceType reqForceType, double reqQuantity, double reqAngle)
    {
        var force = forces?.SingleOrDefault(x => x.ForceType == reqForceType);
        if (force == null)
        {
            Assert.Fail($"Could not find any force of type {reqForceType}");
            return null;
        }

        Assert.Multiple(() =>
        {
            if (double.IsNaN(reqQuantity))
            {
                Assert.That(force.Quantity, Is.NaN, $"{reqForceType} force's quantity must be NaN");
            }
            else
            {
                Assert.That(force.Quantity, Is.EqualTo(reqQuantity).Within(0.00001), $"{reqForceType} force doesn't have correct quantity");
            }
            Assert.That(force.Angle, Is.EqualTo(reqAngle).Within(0.00001), $"{reqForceType} force doesn't have correct angle");
        });

        return force;
    }
}
