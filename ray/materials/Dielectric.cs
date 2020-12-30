using System;
using ray.core;

namespace ray.materials
{
    public class Dielectric : IMaterial
    {
        public double RefractionIndex = 1.0;
        
        public bool Scatter(Ray r, HitRecord hr, out Vec3 attenuation, out Ray scattered)
        {
            attenuation = new Vec3(1, 1, 1);
            var refractionRatio = hr.IsFrontFace ? (1.0 / RefractionIndex) : RefractionIndex;
            var unitDir = r.Dir.Normalized();

            var cosTheta = Math.Min(Vec3.Dot(-unitDir, hr.Normal), 1.0);
            var sinTheta = Math.Sqrt(1.0 - cosTheta * cosTheta);

            var cannotRefract = refractionRatio * sinTheta > 1.0;
            var direction = (cannotRefract || Reflectance(cosTheta, refractionRatio) > MathUtils.RandDouble())? 
                Vec3.Reflect(unitDir, hr.Normal) :
                Vec3.Refract(unitDir, hr.Normal, refractionRatio);
            scattered = new Ray(hr.Point, direction, r.Time);
            return true;
        }

        private double Reflectance(double cosine, double refIdx)
        {
            var r0 = (1 - refIdx) / (1 + refIdx);
            r0 *= r0;
            return r0 + (1 - r0) * Math.Pow((1 - cosine), 5);
        }
    }
}