using System;
using System.Runtime.CompilerServices;
using ray.core;

namespace ray.texture
{
    public class CheckerTexture : ITexture
    {
        public ITexture Odd;
        public ITexture Even;

        public CheckerTexture(ITexture odd, ITexture even)
        {
            Odd = odd;
            Even = even;
        }

        public CheckerTexture(Vec3 odd, Vec3 even)
        {
            Odd = new SolidColor(odd);
            Even = new SolidColor(even);
        }
        
        public Vec3 Value(double u, double v, Vec3 p)
        {
            var sines = Math.Sin(10 * p.x) * Math.Sin(10 * p.y) * Math.Sin(10 * p.z);
            if (sines < 0)
            {
                return Odd.Value(u, v, p);
            }
            else
            {
                return Even.Value(u, v, p);
            }
        }
    }
}