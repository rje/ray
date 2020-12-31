using System.Collections.Generic;
using ray.core;
using ray.hittables;
using ray.materials;
using ray.texture;

namespace ray.worlds
{
    public class EarthTest : IWorldGenerator
    {
        public List<IHittable> Generate()
        {
            var world = new List<IHittable>();
            var earthTex = new ImageTexture("../../Data/earthmap.jpg");
            var earthMat = new Lambertian(earthTex);
            world.Add(new Sphere
            {
                Material = earthMat,
                Center = new Vec3(0, 0, 0),
                Radius = 2
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