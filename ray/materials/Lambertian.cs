using ray.core;
using ray.texture;

namespace ray.materials
{
    public class Lambertian : IMaterial
    {
        public ITexture Texture;

        public Lambertian(Vec3 albedo)
        {
            Texture = new SolidColor{ Color = albedo };
        }

        public Lambertian(ITexture texture)
        {
            Texture = texture;
        }
        
        public override bool Scatter(Ray r, HitRecord hr, out Vec3 attenuation, out Ray scattered)
        {
            var scatterDir = hr.Normal + Vec3.RandomUnitVector();

            if (scatterDir.NearZero())
            {
                scatterDir = hr.Normal;
            }
            
            scattered = new Ray(hr.Point, scatterDir, r.Time);
            attenuation = Texture.Value(hr.U, hr.V, hr.Point);
            return true;
        }
    }
}