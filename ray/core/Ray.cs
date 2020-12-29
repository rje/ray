using System.Threading;

namespace ray.core
{
    public struct Ray
    {
        public Vec3 Origin;
        public Vec3 Dir;

        public static long RayCount = 0;

        public Ray(Vec3 o, Vec3 d)
        {
            Origin = o;
            Dir = d;
        }

        public Vec3 At(double t)
        {
            return Origin + Dir * t;
        }

        public Vec3 GetColor(IHittable world, int depth)
        {
            if (depth <= 0)
            {
                return Vec3.Zero;
            }

            Interlocked.Increment(ref RayCount);
            if (world.Hit(this, 0.001, MathUtils.Infinity, out var hr))
            {
                if (hr.Material.Scatter(this, hr, out var attenuation, out var scattered))
                {
                    return attenuation * scattered.GetColor(world, depth - 1);
                }

                return Vec3.Zero;
            }
            
            var unitDir = Dir.Normalized();
            var t = 0.5 * (unitDir.y + 1.0);
            return new Vec3(1, 1, 1) * (1.0 - t) + new Vec3(0.5, 0.7, 1.0) * t;
        }
    }
}