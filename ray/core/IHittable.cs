namespace ray.core
{
    public interface IHittable
    {
        bool Hit(Ray r, double tMin, double tMax, out HitRecord hr);
        bool BoundingBox(double t0, double t1, out Aabb outputBox);
    }
}