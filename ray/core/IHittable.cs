namespace ray.core
{
    public interface IHittable
    {
        bool Hit(Ray r, double tMin, double tMax, out HitRecord hr);
    }
}