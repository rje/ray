using System;
using ray.core;

namespace ray.hittables
{
    public class Sphere : IHittable
    {
        public Vec3 Center;
        public double Radius;
        public IMaterial Material;
        
        public bool Hit(Ray r, double tMin, double tMax, out HitRecord hitRec)
        {
            var oc = r.Origin - Center;
            var a = r.Dir.LengthSquared;
            var halfB = Vec3.Dot(oc, r.Dir);
            var c = oc.LengthSquared - Radius * Radius;

            var disc = halfB * halfB - a * c;
            if (disc < 0)
            {
                hitRec = default(HitRecord);
                return false;
            }

            var sqrtd = Math.Sqrt(disc);

            var root = (-halfB - sqrtd) / a;
            if (root < tMin || tMax < root)
            {
                root = (-halfB + sqrtd) / a;
                if (root < tMin || tMax < root)
                {
                    hitRec = default(HitRecord);
                    return false;
                }
            }

            var t = root;
            var point = r.At(t);
            var outwardNormal = (point - Center) / Radius;
            hitRec = new HitRecord
            {
                T = t,
                Point = point,
                Material = Material
            };
            hitRec.SetFaceNormal(r, outwardNormal);
            return true;
        }
    }
}