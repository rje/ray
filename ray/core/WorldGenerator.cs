using System;
using System.Collections.Generic;
using ray.hittables;
using ray.materials;

namespace ray.core
{
    public static class WorldGenerator
    {

        public static List<IHittable> RandomScene()
        {
            var world = new List<IHittable>();

            var groundMat = new Lambertian {Albedo = new Vec3(0.5, 0.5, 0.5)};
            world.Add(new Sphere {Center = new Vec3(0, -1000, 0), Radius = 1000, Material = groundMat});
            
            for(var a = -11; a < 11; a++)
            {
                for (var b = -11; b < 11; b++)
                {
                    var chooseMat = MathUtils.RandDouble();
                    var center = new Vec3(
                        a + 0.9 * MathUtils.RandDouble(),
                        0.2,
                        b + 0.9 * MathUtils.RandDouble());

                    if ((center - new Vec3(4, 0.2, 0)).Length <= 0.9)
                    {
                        continue;
                    }

                    if (chooseMat < 0.7)
                    {
                        var albedo = Vec3.Random() * Vec3.Random();
                        var mat = new Lambertian {Albedo = albedo};
                        var c2 = center + new Vec3(0, MathUtils.RandDouble(0, 0.5), 0);
                        world.Add(new MovingSphere {Center0 = center, Center1 = c2, Radius = 0.2, Time0 = 0, Time1 = 1, Material = mat});
                    }
                    else if (chooseMat < 0.95)
                    {
                        var albedo = Vec3.Random(0.5, 1);
                        var fuzz = MathUtils.RandDouble(0, 0.5);
                        var mat = new Metal {Albedo = albedo, Fuzz = fuzz};
                        world.Add(new Sphere {Center = center, Radius = 0.2, Material = mat});
                    }
                    else
                    {
                        var mat = new Dielectric {RefractionIndex = 1.5};
                        world.Add(new Sphere {Center = center, Radius = 0.2, Material = mat});
                    }
                }
            }

            var mat1 = new Dielectric {RefractionIndex = 1.5};
            world.Add(new Sphere {Center = new Vec3(0, 1, 0), Radius = 1.0, Material = mat1});

            var mat2 = new Lambertian {Albedo = new Vec3(0.4, 0.2, 0.1)};
            world.Add(new Sphere {Center = new Vec3(-4, 1, 0), Radius = 1, Material = mat2});

            var mat3 = new Metal {Albedo = new Vec3(0.7, 0.6, 0.5), Fuzz = 0.0};
            world.Add(new Sphere {Center = new Vec3(4, 1, 0), Radius = 1, Material = mat3});

            return world;
        }
    }
}