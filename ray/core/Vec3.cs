using System;
using System.Collections.Generic;

namespace ray.core
{
    public struct Vec3
    {
        private double _x;
        private double _y;
        private double _z;

        public static Vec3 Zero => new Vec3(0, 0, 0);
        public static Vec3 One => new Vec3(1, 1, 1);
        public static Vec3 Up => new Vec3(0, 1, 0);

        public Vec3(double x, double y, double z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public double x
        {
            get => _x;
            set => _x = value;
        }

        public double y
        {
            get => _y;
            set => _y = value;
        }

        public double z
        {
            get => _z;
            set => _z = value;
        }

        public double r => _x;
        public double g => _y;
        public double b => _z;


        public double Length => Math.Sqrt(LengthSquared);
        public double LengthSquared => (x * x) + (y * y) + (z * z);

        public Vec3 Normalized()
        {
            return this / Length;
        }

        public Vec3 GammaCorrected() => new Vec3(Math.Sqrt(x), Math.Sqrt(y), Math.Sqrt(z));

        public bool NearZero()
        {
            const double s = 1e-8;
            return Math.Abs(x) < s && Math.Abs(y) < s && Math.Abs(z) < s;
        }

        public static Vec3 Reflect(Vec3 v, Vec3 n)
        {
            return v - 2 * Vec3.Dot(v, n) * n;
        }

        public static Vec3 Refract(Vec3 uv, Vec3 n, double etaIOverT)
        {
            var cosTheta = Math.Min(Vec3.Dot(-uv, n), 1.0);
            var rOutPerp = etaIOverT * (uv + cosTheta * n);
            var rOutParallel = -Math.Sqrt(Math.Abs(1.0 - rOutPerp.LengthSquared)) * n;
            return rOutPerp + rOutParallel;
        }

        public static Vec3 Random() => new Vec3(
            MathUtils.RandDouble(),
            MathUtils.RandDouble(),
            MathUtils.RandDouble());

        public static Vec3 Random(double min, double max) => new Vec3(
            MathUtils.RandDouble(min, max),
            MathUtils.RandDouble(min, max),
            MathUtils.RandDouble(min, max));

        public static Vec3 RandomInUnitSphere()
        {
            while (true)
            {
                var p = Vec3.Random(-1, 1);
                if (p.LengthSquared < 1)
                {
                    return p;
                }
            }
        }

        public static Vec3 RandomUnitVector()
        {
            return RandomInUnitSphere().Normalized();
        }

        public static Vec3 RandomInHemisphere(Vec3 normal)
        {
            var inSphere = RandomInUnitSphere();
            return Vec3.Dot(normal, inSphere) > 0 ? inSphere : -inSphere;
        }

        public static Vec3 RandomInUnitDisk()
        {
            while (true)
            {
                var p = new Vec3(MathUtils.RandDouble(-1, 1), MathUtils.RandDouble(-1, 1), 0);
                if (p.LengthSquared > 1)
                {
                    continue;
                }

                return p;
            }
        }

        public static List<Vec3> GenerateWhite2DNoise(int cellDim)
        {
            var total = cellDim * cellDim;
            var toReturn = new List<Vec3>();

            for (var i = 0; i < total; i++)
            {
                toReturn.Add(new Vec3(MathUtils.RandDouble(), MathUtils.RandDouble(), 0));
            }

            return toReturn;
        }

        public static List<Vec3> GenerateJittered2DNoise(int cellDim)
        {
            var toReturn = new List<Vec3>();
            var range = 1.0 / cellDim;
            var rangeOver2 = range / 2.0;
            for (var y = 0; y < cellDim; y++)
            {
                var baseY = y * range + rangeOver2;
                for (var x = 0; x < cellDim; x++)
                {
                    var baseX = x * range + rangeOver2;
                    var nx = baseX + MathUtils.RandDouble(-rangeOver2, rangeOver2);
                    var ny = baseY + MathUtils.RandDouble(-rangeOver2, rangeOver2);
                    toReturn.Add(new Vec3(nx, ny, 0));
                }
            }

            return toReturn;
        }

        public double this[int idx]
        {
            get
            {
                switch (idx)
                {
                    case 0: return x;
                    case 1: return y;
                    case 2: return z;
                    default: throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (idx)
                {
                    case 0: _x = value; break;
                    case 1: _y = value; break;
                    case 2: _z = value; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        public static double Dot(Vec3 a, Vec3 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public static Vec3 Cross(Vec3 a, Vec3 b)
        {
            return new Vec3(
                a.y * b.z - a.z * b.y,
                a.z * b.x - a.x * b.z,
                a.x * b.y - a.y * b.x );
        }
        
        public static Vec3 operator +(Vec3 a, Vec3 b)
        {
            return new Vec3(a.x + b.x, a.y + b.y, a.z + b.z);
        }
        
        public static Vec3 operator -(Vec3 a, Vec3 b)
        {
            return new Vec3(a.x - b.x, a.y - b.y, a.z - b.z);
        }
        
        public static Vec3 operator +(Vec3 a, double t)
        {
            return new Vec3(a.x + t, a.y + t, a.z + t);
        }
 
        public static Vec3 operator *(double t, Vec3 a)
        {
            return new Vec3(a.x * t, a.y * t, a.z * t);
        }

        public static Vec3 operator *(Vec3 a, double t)
        {
            return new Vec3(a.x * t, a.y * t, a.z * t);
        }
        
        public static Vec3 operator /(Vec3 a, double t)
        {
            return new Vec3(a.x / t, a.y / t, a.z / t);
        }

        public static Vec3 operator *(Vec3 a, Vec3 b)
        {
            return new Vec3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        public static Vec3 operator -(Vec3 a)
        {
            return a * -1;
        }
        
        public override string ToString()
        {
            return $"({x:0.0000}, {y:0.0000}, {z:0.0000})";
        }

        public static List<double> GenerateConstantSampleWeights(int cellDim)
        {
            var toReturn = new List<double>();
            var totalDim = cellDim * cellDim;
            for (var i = 0; i < totalDim; i++)
            {
                toReturn.Add(1.0 / totalDim);
            }

            return toReturn;
        }
    }
}