using ray.core;

namespace ray.hittables
{
    public class RectYZ : IHittable
    {
        public IMaterial Mp;
        public double Y0, Y1, Z0, Z1, K;

        public RectYZ(double y0, double y1, double z0, double z1, double k, IMaterial mat)
        {
            Y0 = y0;
            Y1 = y1;
            Z0 = z0;
            Z1 = z1;
            K = k;
            Mp = mat;
        }
        
        public bool Hit(Ray r, double tMin, double tMax, out HitRecord hr)
        {
            var t = (K - r.Origin.x) / r.Dir.x;
            if (t < tMin || t > tMax)
            {
                hr = default(HitRecord);
                return false;
            }

            var y = r.Origin.y + t * r.Dir.y;
            var z = r.Origin.z + t * r.Dir.z;

            if (y < Y0 || y > Y1 || z < Z0 || z > Z1)
            {
                hr = default;
                return false;
            }

            var u = (y - Y0) / (Y1 - Y0);
            var v = (z - Z0) / (Z1 - Z0);
            var outwardNormal = new Vec3(1, 0, 0);
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
                Min = new Vec3(K - 0.0001, Y0, Z0),
                Max = new Vec3(K + 0.0001, Y1, Z1)
            };
            return true;
        }
    }
}