using ray.core;

namespace ray.texture
{
    public class SolidColor : ITexture
    {
        public Vec3 Color = new Vec3(0.5, 0.5, 0.5);
        
        public SolidColor() {}

        public SolidColor(Vec3 col)
        {
            Color = col;
        }
        
        public Vec3 Value(double u, double v, Vec3 p)
        {
            return Color;
        }
    }
}