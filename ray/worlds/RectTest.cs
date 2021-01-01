using System.Collections.Generic;
using ray.core;
using ray.hittables;
using ray.materials;
using ray.texture;

namespace ray.worlds
{
    public class RectTest : IWorldGenerator
    {
        public List<IHittable> Generate()
        {
            var tex = new NoiseTexture(Vec3.One, Vec3.Zero, 4);
            var mat = new Lambertian(tex);
            var toReturn = new List<IHittable>();
            
            toReturn.Add(new Sphere
            {
                Center = new Vec3(0, -1000, 0),
                Radius = 1000,
                Material = mat
            });
            
            toReturn.Add(new Sphere
            {
                Center = new Vec3(0, 2, 0),
                Radius = 2,
                Material = mat
            });

            toReturn.Add(
                new RectXY(0, 5, 0, 5, -2, new Lambertian(new Vec3(1, 0, 1)))); 
            return toReturn;
        }

        public Camera GetCamera(double aspect)
        {
            var lookFrom = new Vec3(26, 3, 6);
            var lookAt = new Vec3(0, 2, 0);
            var distToFocus = 10.0;
            var aperture = 0.0;
            var cam = new Camera(
                lookFrom,
                lookAt,
                Vec3.Up,
                new Vec3(0.5, 0.8, 1.0),
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