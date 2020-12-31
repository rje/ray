using System;
using ray.core;

namespace ray.texture
{
    public class NoiseTexture : ITexture
    {
        public Perlin Noise;
        public double Scale;

        public NoiseTexture(double scale = 1)
        {
            Noise = new Perlin();
            Scale = scale;
        }
        
        public Vec3 Value(double u, double v, Vec3 p)
        {
            var noise = 0.5 * (1 + Math.Sin(Scale * p.z + 10 * Noise.Turb(p * Scale)));
            return new Vec3(1, 1, 1) * noise;
        }
    }
}