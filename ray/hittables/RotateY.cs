using System;
using ray.core;

namespace ray.hittables
{
    public class RotateY : IHittable
    {
        public IHittable Child;
        public double SinTheta;
        public double CosTheta;
        public bool HasBox;
        public Aabb BBox;

        public RotateY(IHittable child, double angle)
        {
            Child = child;

            var radians = MathUtils.Deg2Rad(angle);
            SinTheta = Math.Sin(radians);
            CosTheta = Math.Cos(radians);
            HasBox = Child.BoundingBox(0, 1, out var box);

            var min = new Vec3(MathUtils.Infinity, MathUtils.Infinity, MathUtils.Infinity);
            var max = new Vec3(-MathUtils.Infinity, -MathUtils.Infinity, -MathUtils.Infinity);

            for (var i = 0; i < 2; i++)
            {
                for (var j = 0; j < 2; j++)
                {
                    for (var k = 0; k < 2; k++)
                    {
                        var x = i * box.Max.x + (1 - i) * box.Min.x;
                        var y = j * box.Max.y + (1 - j) * box.Min.y;
                        var z = k * box.Max.z + (1 - k) * box.Min.z;

                        var newX = CosTheta * x + SinTheta * z;
                        var newZ = -SinTheta * x + CosTheta * z;

                        var tester = new Vec3(newX, y, newZ);

                        for (int c = 0; c < 3; c++)
                        {
                            min[c] = Math.Min(min[c], tester[c]);
                            max[c] = Math.Max(max[c], tester[c]);
                        }
                    }
                }
            }

            BBox = new Aabb
            {
                Min = min,
                Max = max
            };
        }
        
        public bool Hit(Ray r, double tMin, double tMax, out HitRecord hr)
        {
            var origin = r.Origin;
            var dir = r.Dir;

            origin[0] = CosTheta * r.Origin[0] - SinTheta * r.Origin[2];
            origin[2] = SinTheta * r.Origin[0] + CosTheta * r.Origin[2];

            dir[0] = CosTheta * r.Dir[0] - SinTheta * r.Dir[2];
            dir[2] = SinTheta * r.Dir[0] + CosTheta * r.Dir[2];

            var rotatedRay = new Ray(origin, dir, r.Time);

            if (!Child.Hit(rotatedRay, tMin, tMax, out hr))
            {
                return false;
            }

            var p = hr.Point;
            var normal = hr.Normal;

            p[0] =  CosTheta * hr.Point[0] + SinTheta * hr.Point[2];
            p[2] = -SinTheta * hr.Point[0] + CosTheta * hr.Point[2];

            normal[0] =  CosTheta * hr.Normal[0] + SinTheta * hr.Normal[2];
            normal[2] = -SinTheta * hr.Normal[0] + CosTheta * hr.Normal[2];

            hr.Point = p;
            hr.SetFaceNormal(rotatedRay, normal);
            
            return true;
        }

        public bool BoundingBox(double t0, double t1, out Aabb outputBox)
        {
            outputBox = BBox;
            return HasBox;
        }
    }
}