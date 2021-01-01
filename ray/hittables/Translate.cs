using ray.core;

namespace ray.hittables
{
    public class Translate : IHittable
    {
        public IHittable Child;
        public Vec3 Offset;

        public Translate(IHittable child, Vec3 displacement)
        {
            Child = child;
            Offset = displacement;
        }
        
        public bool Hit(Ray r, double tMin, double tMax, out HitRecord hr)
        {
            var movedRay = new Ray(r.Origin - Offset, r.Dir, r.Time);
            if (!Child.Hit(movedRay, tMin, tMax, out hr))
            {
                return false;
            }

            hr.Point += Offset;
            hr.SetFaceNormal(movedRay, hr.Normal);

            return true;
        }

        public bool BoundingBox(double t0, double t1, out Aabb outputBox)
        {
            if (!Child.BoundingBox(t0, t1, out outputBox))
            {
                return false;
            }

            outputBox = new Aabb
            {
                Min = outputBox.Min + Offset,
                Max = outputBox.Max + Offset
            };
            return true;
        }
    }
}