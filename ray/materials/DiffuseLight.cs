using ray.core;
using ray.texture;

namespace ray.materials
{
    public class DiffuseLight : IMaterial
    {
        public ITexture Emit;

        public DiffuseLight(ITexture emit)
        {
            Emit = emit;
        }

        public DiffuseLight(Vec3 color)
        {
            Emit = new SolidColor(color);
        }
        
        public override bool Scatter(Ray r, HitRecord hr, out Vec3 attenuation, out Ray scattered)
        {
            attenuation = default;
            scattered = default;
            return false;
        }

        public override Vec3 Emitted(double u, double v, Vec3 p)
        {
            return Emit.Value(u, v, p);
        }
    }
}