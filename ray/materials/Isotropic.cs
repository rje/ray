using ray.core;
using ray.texture;

namespace ray.materials
{
    public class Isotropic : IMaterial
    {
        public ITexture Albedo;

        public Isotropic(ITexture a)
        {
            Albedo = a;
        }

        public Isotropic(Vec3 c)
        {
            Albedo = new SolidColor(c);
        }
        
        public override bool Scatter(Ray r, HitRecord hr, out Vec3 attenuation, out Ray scattered)
        {
            scattered = new Ray(hr.Point, Vec3.RandomInUnitSphere(), r.Time);
            attenuation = Albedo.Value(hr.U, hr.V, hr.Point);
            return true;
        }
    }
}