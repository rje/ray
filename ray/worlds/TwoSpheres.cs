using System.Collections.Generic;
using ray.core;
using ray.hittables;
using ray.materials;
using ray.texture;

namespace ray.worlds
{
    public class TwoSpheres : IWorldGenerator
    {
        public List<IHittable> Generate()
        {
            var world = new List<IHittable>();
            var checker = new CheckerTexture(new Vec3(0.2, 0.3, 0.1), new Vec3(0.9, 0.9, 0.9));
            world.Add(new Sphere
            {
                Material = new Lambertian(checker),
                Center = new Vec3(0, -10, 0),
                Radius = 10
            });
            world.Add(new Sphere
            {
                Material = new Lambertian(checker),
                Center = new Vec3(0, 10, 0),
                Radius = 10
            });

            return world;
        }

        public Camera GetCamera(double aspect)
        {
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