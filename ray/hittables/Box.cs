using ray.core;
using ray.materials;
using ray.texture;

namespace ray.hittables
{
    public class Box : IHittable
    {
        public Vec3 Min;
        public Vec3 Max;
        public HittableList Sides;

        public Box(Vec3 p0, Vec3 p1, Vec3 color) :
            this (p0, p1, new Lambertian(color)) {
        }

        public Box(Vec3 p0, Vec3 p1, IMaterial mat)
        {
            Min = p0;
            Max = p1;
            Sides = new HittableList();

            Sides.Add(new RectXY(p0.x, p1.x, p0.y, p1.y, p0.z, mat));
            Sides.Add(new RectXY(p0.x, p1.x, p0.y, p1.y, p1.z, mat));

            Sides.Add(new RectXZ(p0.x, p1.x, p0.z, p1.z, p0.y, mat));
            Sides.Add(new RectXZ(p0.x, p1.x, p0.z, p1.z, p1.y, mat));

            Sides.Add(new RectYZ(p0.y, p1.y, p0.z, p1.z, p0.x, mat));
            Sides.Add(new RectYZ(p0.y, p1.y, p0.z, p1.z, p1.x, mat));
        }

        public bool Hit(Ray r, double tMin, double tMax, out HitRecord hr)
        {
            var toReturn = Sides.Hit(r, tMin, tMax, out var hitRec);
            hr = hitRec;
            return toReturn;
        }

        public bool BoundingBox(double t0, double t1, out Aabb outputBox)
        {
            outputBox = new Aabb {Min = Min, Max = Max};
            return true;
        }
    }
}