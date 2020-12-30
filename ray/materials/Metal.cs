using ray.core;

namespace ray.materials
{
    public class Metal : IMaterial
    {
        public Vec3 Albedo = new Vec3(0.5, 0.5, 0.5);
        public double Fuzz = 0.5;
        
        public bool Scatter(Ray r, HitRecord hr, out Vec3 attenuation, out Ray scattered)
        {
            var reflected = Vec3.Reflect(r.Dir.Normalized(), hr.Normal);
            scattered = new Ray(hr.Point, reflected + Fuzz * Vec3.RandomInUnitSphere(), r.Time);
            attenuation = Albedo;
            return Vec3.Dot(scattered.Dir, hr.Normal) > 0;
        }
    }
}