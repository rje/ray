using System.Collections.Generic;
using ray.core;
using ray.hittables;
using ray.materials;
using ray.texture;

namespace ray.worlds
{
    public class PerlinTest : IWorldGenerator
    {
        public List<IHittable> Generate()
        {
            var toReturn = new List<IHittable>();

            var perTex = new NoiseTexture(4);
            toReturn.Add(new Sphere
            {
                Center = new Vec3(0, -1000, 0),
                Radius = 1000,
                Material = new Lambertian(perTex)
            });
            
            toReturn.Add(new Sphere
            {
                Center = new Vec3(0, 2, 0),
                Radius = 2,
                Material = new Lambertian(perTex)
            });
            return toReturn;
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