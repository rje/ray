using System;
using System.Threading;

namespace ray.core
{
    public static class MathUtils
    {
        public const double PiOver180 = Math.PI / 180.0;
        public const double Infinity = double.PositiveInfinity;

        private static Random _global = new Random();
        private static ThreadLocal<Random> _rand = new ThreadLocal<Random>(() =>
        {
            lock (_global)
            {
                return new Random(_global.Next());
            }
        });
        
        public static double Deg2Rad(double deg)
        {
            return deg * PiOver180;
        }

        public static double Rad2Deg(double rad)
        {
            return rad / PiOver180;
        }

        public static double RandDouble()
        {
            return _rand.Value.NextDouble();
        }

        public static double RandDouble(double min, double max)
        {
            return min + (max - min) * RandDouble();
        }

        public static double Clamp(double x, double min, double max)
        {
            if (x < min)
            {
                return min;
            }

            if (x > max)
            {
                return max;
            }

            return x;
        }

        public static int RandomInt(int min, int maxExcl)
        {
            return _rand.Value.Next(min, maxExcl);
        }
    }
}