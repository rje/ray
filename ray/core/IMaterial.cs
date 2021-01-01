namespace ray.core
{
    public abstract class IMaterial
    {
        public virtual Vec3 Emitted(double u, double v, Vec3 p)
        {
            return Vec3.Zero;
        }
        
        public abstract bool Scatter(Ray r, HitRecord hr, out Vec3 attenuation, out Ray scattered);
    }
}