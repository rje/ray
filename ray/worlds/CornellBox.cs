using System.Collections.Generic;
using ray.core;
using ray.hittables;
using ray.materials;
using ray.texture;

namespace ray.worlds
{
    public class CornellBox : IWorldGenerator
    {
        public List<IHittable> Generate()
        {
            var toReturn = new List<IHittable>();

            var red = new Lambertian(new Vec3(0.65, 0.05, 0.05));
            var white = new Lambertian(new Vec3(0.73, 0.73, 0.73));
            var green = new Lambertian(new Vec3(0.12, 0.45, 0.15));
            var light = new DiffuseLight(new Vec3(15, 15, 15));
            
            toReturn.Add(new RectYZ(0, 555, 0, 555, 555, green));
            toReturn.Add(new RectYZ(0, 555, 0, 555, 0, red));
            
            toReturn.Add(new RectXZ(213, 343, 227, 332, 554, light));
            toReturn.Add(new RectXZ(0, 555, 0, 555, 0, white));
            toReturn.Add(new RectXZ(0, 555, 0, 555, 555, white));
            
            toReturn.Add(new RectXY(0, 555, 0, 555, 555, white));
            
            IHittable box1 = new Box(
                new Vec3(0, 0, 0),
                new Vec3(165, 330, 165),
                white);
            box1 = new RotateY(box1, 15);
            box1 = new Translate(box1, new Vec3(265, 0, 295));
            toReturn.Add(box1);
            
            IHittable box2 = new Box(
                new Vec3(0, 0, 0),
                new Vec3(165, 165, 165),
                white);
            box2 = new RotateY(box2, -18);
            box2 = new Translate(box2, new Vec3(130, 0, 65));
            toReturn.Add(box2);
            
            return toReturn;
        }

        public Camera GetCamera(double aspect)
        {
            var lookFrom = new Vec3(278, 278, -800);
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