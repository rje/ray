using System.Collections.Generic;
using ray.core;
using ray.hittables;
using ray.materials;
using ray.texture;

namespace ray.worlds
{
    public class RandomWorld : IWorldGenerator
    {
        public List<IHittable> Generate()
        {
            var world = new List<IHittable>();

            var checker = new CheckerTexture(new Vec3(0.2, 0.3, 0.1), new Vec3(0.9, 0.9, 0.9));
            var groundMat = new Lambertian(checker);
            world.Add(new Sphere {Center = new Vec3(0, -1000, 0), Radius = 1000, Material = groundMat});
            
            var earthTex = new ImageTexture("../../Data/earthmap.jpg");
            var earthMat = new Lambertian(earthTex);
            
            for(var a = -11; a < 11; a++)
            {
                for (var b = -11; b < 11; b++)
                {
                    var chooseMat = MathUtils.RandDouble();
                    var radius = MathUtils.RandDouble(0.2, 0.3);
                    var offset = 1.0 - (radius / 2);
                    var center = new Vec3(
                        a + offset * MathUtils.RandDouble(),
                        radius,
                        b + offset * MathUtils.RandDouble());

                    if ((center - new Vec3(4, radius, 0)).Length <= 0.9)
                    {
                        continue;
                    }

                    if (chooseMat < 0.2)
                    {
                        var albedo = Vec3.Random() * Vec3.Random();
                        var mat = new Lambertian(albedo);
                        var c2 = center + new Vec3(0, MathUtils.RandDouble(0, 0.2), 0);
                        world.Add(new MovingSphere {Center0 = center, Center1 = c2, Radius = radius, Time0 = 0, Time1 = 1, Material = mat});
                    }
                    else if (chooseMat < 0.4)
                    {
                        world.Add(new Sphere {Center = center, Radius = radius, Material = earthMat});
                    }
                    else if (chooseMat < 0.6)
                    {
                        var color1= Vec3.Random() * Vec3.Random();
                        var color2 = Vec3.One - color1;
                        var scale = MathUtils.RandomInt(3, 10);
                        var mat = new Lambertian(new NoiseTexture(color1, color2, scale));
                        world.Add(new Sphere {Center = center, Radius = radius, Material = mat});
                    }
                    else if (chooseMat < 0.8)
                    {
                        var albedo = Vec3.Random(0.5, 1);
                        var fuzz = MathUtils.RandDouble(0, 0.5);
                        var mat = new Metal {Albedo = albedo, Fuzz = fuzz};
                        world.Add(new Sphere {Center = center, Radius = radius, Material = mat});
                    }
                    else
                    {
                        var mat = new Dielectric {RefractionIndex = 1.5};
                        world.Add(new Sphere {Center = center, Radius = radius, Material = mat});
                    }
                }
            }

            var mat1 = new Dielectric {RefractionIndex = 1.5};
            var mat2 = new Lambertian(new Vec3(0.4, 0.2, 0.1));
            var mat3 = new Metal {Albedo = new Vec3(0.7, 0.6, 0.5), Fuzz = 0.0};
            
            world.Add(new Sphere {Center = new Vec3(0, 1, 0), Radius = 1.0, Material = mat1});
            world.Add(new Sphere {Center = new Vec3(-4, 1, 0), Radius = 1, Material = mat2});
            world.Add(new Sphere {Center = new Vec3(4, 1, 0), Radius = 1.0, Material = mat3});

            return world;
        }
        
        public Camera GetCamera(double aspect) {
            var lookFrom = new Vec3(13, 2, 3);
            var lookAt = new Vec3(0, 0, 0);
            var distToFocus = 10.0;
            var aperture = 0.1;
            var cam = new Camera(
                        lookFrom,
                        lookAt,
                        Vec3.Up, 
                        20, 
                        aspect,
                        aperture,
                        distToFocus,
                        0,
                        1
                        );
            return cam;
        }
    }
}