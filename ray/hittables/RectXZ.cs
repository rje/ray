using ray.core;

namespace ray.hittables
{
    public class RectXZ : IHittable
    {
        public IMaterial Mp;
        public double X0, X1, Z0, Z1, K;

        public RectXZ(double x0, double x1, double z0, double z1, double k, IMaterial mat)
        {
            X0 = x0;
            X1 = x1;
            Z0 = z0;
            Z1 = z1;
            K = k;
            Mp = mat;
        }
        
        public bool Hit(Ray r, double tMin, double tMax, out HitRecord hr)
        {
            var t = (K - r.Origin.y) / r.Dir.y;
            if (t < tMin || t > tMax)
            {
                hr = default(HitRecord);
                return false;
            }

            var x = r.Origin.x + t * r.Dir.x;
            var z = r.Origin.z + t * r.Dir.z;

            if (x < X0 || x > X1 || z < Z0 || z > Z1)
            {
                hr = default;
                return false;
            }

            var u = (x - X0) / (X1 - X0);
            var v = (z - Z0) / (Z1 - Z0);
            var outwardNormal = new Vec3(0, 1, 0);
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
                Min = new Vec3(X0, K - 0.0001, Z0),
                Max = new Vec3(X1, K + 0.0001, Z1)
            };
            return true;
        }
    }
}