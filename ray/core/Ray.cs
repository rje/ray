using System.Threading;

namespace ray.core
{
    public struct Ray
    {
        public Vec3 Origin;
        public Vec3 Dir;
        public double Time;

        public static long RayCount = 0;

        public Ray(Vec3 o, Vec3 d, double time = 0)
        {
            Origin = o;
            Dir = d;
            Time = time;
        }

        public Vec3 At(double t)
        {
            return Origin + Dir * t;
        }

        public Vec3 GetColor(IHittable world, int depth, Vec3 background)
        {
            if (depth <= 0)
            {
                return Vec3.Zero;
            }

            Interlocked.Increment(ref RayCount);
            if (!world.Hit(this, 0.001, MathUtils.Infinity, out var hr))
            {
                return background;
            }

            var emitted = hr.Material.Emitted(hr.U, hr.V, hr.Point);

            if(!hr.Material.Scatter(this, hr, out var attenuation, out var scattered))
            {
                return emitted;
            }
            
            return emitted + attenuation * scattered.GetColor(world, depth - 1, background);
            
        }
    }
}