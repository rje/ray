using System;
using System.Collections.Generic;
using ray.core;

namespace ray.hittables
{
    public class HittableList : IHittable
    {
        public List<IHittable> Objects = new List<IHittable>();

        public void Add(IHittable toAdd)
        {
            Objects.Add(toAdd);
        }
        
        public bool Hit(Ray r, double tMin, double tMax, out HitRecord hr)
        {
            var tempRec = default(HitRecord);
            var hitAnything = false;
            var closest = tMax;
            
            for(var i = 0; i < Objects.Count; i++)
            {
                var obj = Objects[i];
                if (obj.Hit(r, tMin, tMax, out var recToCheck))
                {
                    if (recToCheck.T < closest)
                    {
                        hitAnything = true;
                        closest = recToCheck.T;
                        tempRec = recToCheck;
                    }
                }
            }

            hr = tempRec;
            if (hitAnything && hr.Material == null) 
                Console.WriteLine("Returning HittableList hitrec with null material");
            return hitAnything;
        }

        public bool BoundingBox(double t0, double t1, out Aabb outputBox)
        {
            var firstBox = true;
            outputBox = default(Aabb);
            
            foreach(var obj in Objects)
            {
                if (!obj.BoundingBox(t0, t1, out var output))
                {
                    return false;
                }

                outputBox = firstBox ? output : Aabb.SurroundingBox(outputBox, output);
                firstBox = false;
            }

            return true;
        }
    }
}