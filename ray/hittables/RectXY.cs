using ray.core;

namespace ray.hittables
{
    public class RectXY : IHittable
    {
        public IMaterial Mp;
        public double X0, X1, Y0, Y1, K;

        public RectXY(double x0, double x1, double y0, double y1, double k, IMaterial mat)
        {
            X0 = x0;
            X1 = x1;
            Y0 = y0;
            Y1 = y1;
            K = k;
            Mp = mat;
        }
        
        public bool Hit(Ray r, double tMin, double tMax, out HitRecord hr)
        {
            var t = (K - r.Origin.z) / r.Dir.z;
            if (t < tMin || t > tMax)
            {
                hr = default(HitRecord);
                return false;
            }

            var x = r.Origin.x + t * r.Dir.x;
            var y = r.Origin.y + t * r.Dir.y;

            if (x < X0 || x > X1 || y < Y0 || y > Y1)
            {
                hr = default;
                return false;
            }

            var u = (x - X0) / (X1 - X0);
            var v = (y - Y0) / (Y1 - Y0);
            var outwardNormal = new Vec3(0, 0, 1);
            hr = new HitRecord
            {
                T = t,
                Point = r.At(t),
                Material = Mp,
                U = u,
                V = v
            };
            hr.SetFaceNormal(r, outwardNormal);
            return true;
        }

        public bool BoundingBox(double t0, double t1, out Aabb outputBox)
        {
            outputBox = new Aabb
            {
                Min = new Vec3(X0, Y0, K - 0.0001),
                Max = new Vec3(X1, Y1, K + 0.0001)
            };
            return true;
        }
    }
}