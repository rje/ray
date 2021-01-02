using System.Collections.Generic;
using ray.core;
using ray.hittables;
using ray.materials;
using ray.texture;

namespace ray.worlds
{
    public class NextWeekScene : IWorldGenerator
    {
        public List<IHittable> Generate()
        {
            var toReturn = new List<IHittable>();

            var ground = new Lambertian(new Vec3(0.48, 0.83, 0.53));
            const int boxesPerSide = 20;
            var boxes1 = new List<IHittable>();
            for (var i = 0; i < boxesPerSide; i++)
            {
                for (var j = 0; j < boxesPerSide; j++)
                {
                    var w = 100.0;
                    var x0 = -1000 + i * w;
                    var z0 = -1000 + j * w;
                    var y0 = 0;
                    var x1 = x0 + w;
                    var y1 = MathUtils.RandDouble(1, 101);
                    var z1 = z0 + w;
                    boxes1.Add(new Box(
                        new Vec3(x0, y0, z0),
                        new Vec3(x1, y1, z1),
                        ground));
                }
            }
            toReturn.Add(new BvhNode(boxes1, 0, boxes1.Count, 0, 1));

            var light = new DiffuseLight(new Vec3(7, 7, 7));
            toReturn.Add(new RectXZ(123, 423, 147, 412, 554, light));

            var c1 = new Vec3(400, 400, 200);
            var c2 = c1 + new Vec3(30, 0, 0);
            var movingSphereMat = new Lambertian(new Vec3(0.7, 0.3, 0.1));
            toReturn.Add(new MovingSphere { Center0 = c1, Center1 = c2, Radius = 50, Time0 = 0, Time1 = 1, Material = movingSphereMat});
            
            toReturn.Add(new Sphere {Center = new Vec3(260, 150, 45), Radius = 50, Material = new Dielectric { RefractionIndex = 1.5}});
            toReturn.Add(new Sphere
            {
                Center = new Vec3(0, 150, 145),
                Radius = 50,
                Material = new Metal {Albedo = new Vec3(0.8, 0.8, 0.9), Fuzz = 1.0 }
            });

            var boundary = new Sphere
            {
                Center = new Vec3(360, 150, 145), Radius = 70,
                Material = new Dielectric { RefractionIndex = 1.5 }
            };
            toReturn.Add(boundary);
            toReturn.Add(new ConstantMedium(boundary, 0.2, new Vec3(0.2, 0.4, 0.9)));

            boundary = new Sphere
            {
                Center = Vec3.Zero,
                Radius = 5000,
                Material = new Dielectric {RefractionIndex = 1.5}
            };
            toReturn.Add(new ConstantMedium(boundary, 0.0001, Vec3.One));

            var emat = new Lambertian(new ImageTexture("../../Data/earthmap.jpg"));
            toReturn.Add(new Sphere
            {
                Center = new Vec3(400, 200, 400),
                Radius = 100,
                Material = emat
            });

            var pertex = new NoiseTexture(0.1);
            toReturn.Add(new Sphere
            {
                Center = new Vec3(220, 280, 300),
                Radius = 80,
                Material = new Lambertian(pertex)
            });

            var boxes2 = new List<IHittable>();
            var white = new Lambertian(new Vec3(0.73, 0.73, 0.73));
            var ns = 1000;
            for (int j = 0; j < ns; j++)
            {
                boxes2.Add(new Sphere
                {
                    Center = Vec3.Random(0, 165),
                    Radius = 10,
                    Material = white
                });
            }
            
            toReturn.Add(new Translate(
                new RotateY(
                    new BvhNode(boxes2, 0, boxes2.Count, 0, 1),
                    15
                    ),
                new Vec3(-100, 270, 395)
                ));
            

            return toReturn;
        }

        public Camera GetCamera(double aspect)
        {
            var lookFrom = new Vec3(478, 278, -600);
            var lookAt = new Vec3(278, 278, 0);
            var distToFocus = 10.0;
            var aperture = 0.0;
            var cam = new Camera(
                lookFrom,
                lookAt,
                Vec3.Up,
                Vec3.Zero,
                40, 
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