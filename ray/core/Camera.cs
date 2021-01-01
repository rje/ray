using System;

namespace ray.core
{
    public class Camera
    {
        public Vec3 Origin;
        public Vec3 Background;
        
        private Vec3 LowerLeftCorner;
        private Vec3 Horziontal;
        private Vec3 Vertical;
        private double LensRadius;
        private Vec3 u, v, w;
        private double Time0, Time1;

        public Camera(
            Vec3 lookFrom, 
            Vec3 lookAt, 
            Vec3 vUp, 
            Vec3 background,
            double vFov, 
            double aspect, 
            double aperture, 
            double focusDist,
            double time0 = 0,
            double time1 = 0)
        {
            var theta = MathUtils.Deg2Rad(vFov);
            var h = Math.Tan(theta / 2);
            var viewportHeight = 2.0 * h;
            var viewportWidth = aspect * viewportHeight;

            w = (lookFrom - lookAt).Normalized();
            u = Vec3.Cross(vUp, w).Normalized();
            v = Vec3.Cross(w, u);

            Origin = lookFrom;
            Horziontal = focusDist * viewportWidth * u;
            Vertical = focusDist * viewportHeight * v;
            LowerLeftCorner = Origin - Horziontal / 2 - Vertical / 2 - focusDist * w;
            
            Background = background;
            LensRadius = aperture / 2;
            Time0 = time0;
            Time1 = time1;
        }

        public Ray GetRay(double s, double t)
        {
            var rd = LensRadius * Vec3.RandomInUnitDisk();
            var offset = u * rd.x + v * rd.y;
            return new Ray
            (
                Origin + offset,
                LowerLeftCorner + s * Horziontal + t * Vertical - Origin - offset,
                MathUtils.RandDouble(Time0, Time1)
            );
        }
    }
}