namespace ray.core
{
    public interface IMaterial
    {
        bool Scatter(Ray r, HitRecord hr, out Vec3 attenuation, out Ray scattered);
    }
}