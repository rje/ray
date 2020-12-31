using System;
using ray.core;

namespace ray.texture
{
    public class NoiseTexture : ITexture
    {
        public Perlin Noise;
        public double Scale;
        public Vec3 Color1;
        public Vec3 Color2;

        public NoiseTexture(Vec3 color1, Vec3 color2, double scale = 1)
        {
            Noise = new Perlin();
            Scale = scale;
            Color1 = color1;
            Color2 = color2;
        }
 
        public NoiseTexture(double scale = 1)
        {
            Noise = new Perlin();
            Scale = scale;
            Color1 = Vec3.One;
            Color2 = Vec3.Zero;
        }       
        public Vec3 Value(double u, double v, Vec3 p)
        {
            var noise = 0.5 * (1 + Math.Sin(Scale * p.z + 10 * Noise.Turb(p * Scale)));
            return Color1 * noise + Color2 * (1 - noise);
        }
    }
}