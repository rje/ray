using ray.core;

namespace ray.texture
{
    public interface ITexture
    {
        Vec3 Value(double u, double v, Vec3 p);
    }
}