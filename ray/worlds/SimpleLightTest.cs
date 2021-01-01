using System.Collections.Generic;
using ray.core;
using ray.hittables;
using ray.materials;
using ray.texture;
using StbImageSharp;

namespace ray.worlds
{
    public class SimpleLightTest : IWorldGenerator
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

            var diffLight = new DiffuseLight(new Vec3(4, 4, 4));
            toReturn.Add(new RectXY(3, 5, 1, 3, -2, diffLight));
            
            toReturn.Add(new Sphere
            {
                Center = new Vec3(0, 7, 0),
                Radius = 2,
                Material = diffLight
            });

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
                        Vec3.Zero,
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