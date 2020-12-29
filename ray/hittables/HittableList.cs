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
            
            foreach(var obj in Objects)
            {
                if (obj.Hit(r, tMin, tMax, out var recToCheck))
                {
                    hitAnything = true;
                    if (recToCheck.T < closest)
                    {
                        closest = recToCheck.T;
                        tempRec = recToCheck;
                    }
                }
            }

            hr = tempRec;
            return hitAnything;
        }
    }
}