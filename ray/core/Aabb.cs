using System;

namespace ray.core
{
    public struct Aabb
    {
        public Vec3 Min;
        public Vec3 Max;

        public bool Hit(Ray r, double tMin, double tMax)
        {
            for (var a = 0; a < 3; a++)
            {
                var invD = 1.0f / r.Dir[a];
                var t0 = (Min[a] - r.Origin[a]) * invD;
                var t1 = (Max[a] - r.Origin[a]) * invD;

                if (invD < 0)
                {
                    var tmp = t0;
                    t0 = t1;
                    t1 = tmp;
                }

                tMin = t0 > tMin ? t0 : tMin;
                tMax = t1 < tMax ? t1 : tMax;
                if (tMax <= tMin)
                {
                    return false;
                }
            }

            return true;
        }

        public static Aabb SurroundingBox(Aabb box0, Aabb box1)
        {
            var min = new Vec3(
                Math.Min(box0.Min.x, box1.Min.x),
                Math.Min(box0.Min.y, box1.Min.y),
                Math.Min(box0.Min.z, box1.Min.z)
            );
            var max = new Vec3(
                Math.Max(box0.Max.x, box1.Max.x),
                Math.Max(box0.Max.y, box1.Max.y),
                Math.Max(box0.Max.z, box1.Max.z)
            );
            return new Aabb { Min = min, Max = max };
        }
    }
}