using System;
using System.Collections.Generic;
using System.Diagnostics;
using ray.core;

namespace ray.hittables
{
    public class BvhNode : IHittable
    {
        public IHittable Left;
        public IHittable Right;
        public Aabb Box;

        public BvhNode(List<IHittable> srcObjects, int start, int end, double t0, double t1)
        {
            var axis = MathUtils.RandomInt(0, 3);
            IComparer<IHittable> comparator;
            switch (axis)
            {
                case 0: comparator = new BoxCompareX(); break;
                case 1: comparator = new BoxCompareY(); break;
                default: comparator = new BoxCompareZ(); break;
            }
            
            var objSpan = end - start;

            if (objSpan == 1)
            {
                Left = srcObjects[start];
                Right = srcObjects[start];
            }
            else if (objSpan == 2)
            {
                if (comparator.Compare(srcObjects[start], srcObjects[start + 1]) < 0)
                {
                    Left = srcObjects[start];
                    Right = srcObjects[start + 1];
                }
                else
                {
                    Left = srcObjects[start + 1];
                    Right = srcObjects[start];
                }
            }
            else
            {
                srcObjects.Sort(start, objSpan, comparator);
                var mid = start + objSpan / 2;
                Left = new BvhNode(srcObjects, start, mid, t0, t1);
                Right = new BvhNode(srcObjects, mid, end, t0, t1);
            }

            if (!Left.BoundingBox(t0, t1, out var boxLeft) ||
                !Right.BoundingBox(t0, t1, out var boxRight))
            {
                throw new Exception("No bounding box in BvhNode constructor");
            }
            Box = Aabb.SurroundingBox(boxLeft, boxRight);
        }

        private static int BoxCompare(IHittable a, IHittable b, int axis)
        {
            if (!a.BoundingBox(0, 1, out var boxA) ||
                !b.BoundingBox(0, 1, out var boxB))
            {
                throw new Exception("No bounding box found in BoxCompare!");
            }

            return boxA.Min[axis].CompareTo(boxB.Min[axis]);
        }

        private class BoxCompareX : IComparer<IHittable>
        {
            public int Compare(IHittable x, IHittable y)
            {
                return BvhNode.BoxCompare(x, y, 0);
            }
        }
 
        private class BoxCompareY : IComparer<IHittable>
        {
            public int Compare(IHittable x, IHittable y)
            {
                return BvhNode.BoxCompare(x, y, 1);
            }
        }
        
        private class BoxCompareZ : IComparer<IHittable>
        {
            public int Compare(IHittable x, IHittable y)
            {
                return BvhNode.BoxCompare(x, y, 2);
            }
        }
               
        public bool Hit(Ray r, double tMin, double tMax, out HitRecord hr)
        {
            if (!Box.Hit(r, tMin, tMax))
            {
                hr = default(HitRecord);
                return false;
            }

            var hitLeft = Left.Hit(r, tMin, tMax, out var hrl);
            var hitRight = Right.Hit(r, tMin, hitLeft ? hrl.T : tMax, out var hrr);

            if (hitRight)
            {
                hr = hrr;
                if (hr.Material == null) Console.WriteLine("Returning HRR with null material");
                return true;
            }

            if (hitLeft)
            {
                hr = hrl;
                if (hr.Material == null) Console.WriteLine("Returning HRL with null material");
                return true;
            }

            hr = default(HitRecord);
            return false;
        }

        public bool BoundingBox(double t0, double t1, out Aabb outputBox)
        {
            outputBox = Box;
            return true;
        }
    }
}