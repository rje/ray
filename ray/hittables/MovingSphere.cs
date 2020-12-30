using System;
using ray.core;

namespace ray.hittables
{
    public class MovingSphere : IHittable
    {
        public Vec3 Center0, Center1;
        public double Radius;
        public IMaterial Material;
        public double Time0, Time1;

        private Vec3 _centerChange;
        private bool _first = true;

        public Vec3 CenterForTime(double t)
        {
            if (_first)
            {
                _first = false;
                _centerChange = Center1 - Center0;
            }
            var inc = (t - Time0) / (Time1 - Time0);
            return Center0 + inc * _centerChange;
        }
        
        public bool Hit(Ray r, double tMin, double tMax, out HitRecord hitRec)
        {
            var center = CenterForTime(r.Time);
            var oc = r.Origin - center;
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
            var outwardNormal = (point - center) / Radius;
            Sphere.GetSphereUV(outwardNormal,out var u, out var v);
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
            var c0 = CenterForTime(t0);
            var c1 = CenterForTime(t1);

            var b0 = new Aabb
            {
                Min = c0 - new Vec3(Radius, Radius, Radius),
                Max = c0 + new Vec3(Radius, Radius, Radius),
            };
            
            var b1 = new Aabb
            {
                Min = c1 - new Vec3(Radius, Radius, Radius),
                Max = c1 + new Vec3(Radius, Radius, Radius),
            };

            outputBox = Aabb.SurroundingBox(b0, b1);
            return true;
        }
    }
}