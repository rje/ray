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
            GetSphereUV(outwardNormal,out var u, out var v);
            hitRec = new HitRecord
            {
                T = t,
                Point = point,
                Material = Material,
                U = u,
                V = v
            };
            hitRec.SetFaceNormal(r, outwardNormal);
            return true;
        }

        public bool BoundingBox(double t0, double t1, out Aabb outputBox)
        {
            outputBox = new Aabb
            {
                Min = Center - new Vec3(Radius, Radius, Radius),
                Max = Center + new Vec3(Radius, Radius, Radius),
            };
            return true;
        }

        public static void GetSphereUV(Vec3 p, out double u, out double v)
        {
            var theta = Math.Acos(-p.y);
            var phi = Math.Atan2(-p.z, p.x) + Math.PI;
            u = phi / (2 * Math.PI);
            v = theta / Math.PI;
        }
    }
}