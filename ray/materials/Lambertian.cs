using ray.core;

namespace ray.materials
{
    public class Lambertian : IMaterial
    {
        public Vec3 Albedo = new Vec3(0.5, 0.5, 0.5);
        
        public bool Scatter(Ray r, HitRecord hr, out Vec3 attenuation, out Ray scattered)
        {
            var scatterDir = hr.Normal + Vec3.RandomUnitVector();

            if (scatterDir.NearZero())
            {
                scatterDir = hr.Normal;
            }
            
            scattered = new Ray(hr.Point, scatterDir, r.Time);
            attenuation = Albedo;
            return true;
        }
    }
}