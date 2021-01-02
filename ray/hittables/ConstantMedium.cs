using System;
using System.Windows.Markup;
using ray.core;
using ray.materials;
using ray.texture;

namespace ray.hittables
{
    public class ConstantMedium : IHittable
    {

        public IHittable Boundary;
        public double NegInvDensity;
        public IMaterial PhaseFunction;

        public ConstantMedium(IHittable b, double d, ITexture a)
        {
            Boundary = b;
            NegInvDensity = -1 / d;
            PhaseFunction = new Isotropic(a);
        }
        
        public ConstantMedium(IHittable b, double d, Vec3 color)
        {
            Boundary = b;
            NegInvDensity = -1 / d;
            PhaseFunction = new Isotropic(color);
        }
        
        public bool Hit(Ray r, double tMin, double tMax, out HitRecord hr)
        {
            hr = default;
            if (!Boundary.Hit(r, -MathUtils.Infinity, MathUtils.Infinity, out var hr1))
            {
                return false;
            }

            if (!Boundary.Hit(r, hr1.T + 0.0001, MathUtils.Infinity, out var hr2))
            {
                return false;
            }

            if (hr1.T < tMin) hr1.T = tMin;
            if (hr2.T > tMax) hr2.T = tMax;

            if (hr1.T >= hr2.T)
            {
                return false;
            }

            if (hr1.T < 0)
            {
                hr1.T = 0;
            }

            var rayLen = r.Dir.Length;
            var distInsideBoundary = (hr2.T - hr1.T) * rayLen;
            var hitDist = NegInvDensity * Math.Log(MathUtils.RandDouble());

            if (hitDist > distInsideBoundary)
            {
                return false;
            }
            
            var t = hr1.T + hitDist / rayLen;
            var p = r.At(t);

            hr = new HitRecord
            {
                Point = p,
                T = t,
                IsFrontFace = true,
                Material = PhaseFunction,
                Normal = new Vec3(1, 0, 0),
                U = 0,
                V = 0
            };
            return true;
        }

        public bool BoundingBox(double t0, double t1, out Aabb outputBox)
        {
            return Boundary.BoundingBox(t0, t1, out outputBox);
        }
    }
}