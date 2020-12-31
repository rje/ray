using System;
using ray.core;

namespace ray.texture
{
    public class Perlin
    {
        private const int PointCount = 256;
        private Vec3[] ranVec;
        private int[] permX;
        private int[] permY;
        private int[] permZ;

        public Perlin()
        {
            ranVec = new Vec3[PointCount];

            for (var i = 0; i < PointCount; i++)
            {
                ranVec[i] = Vec3.Random(-1, 1).Normalized();
            }

            permX = GeneratePerm();
            permY = GeneratePerm();
            permZ = GeneratePerm();
        }

        public double Noise(Vec3 p)
        {
            var u = p.x - Math.Floor(p.x);
            var v = p.y - Math.Floor(p.y);
            var w = p.z - Math.Floor(p.z);

            var i = (int) Math.Floor(p.x);
            var j = (int) Math.Floor(p.y);
            var k = (int) Math.Floor(p.z);

            var c = new Vec3[2, 2, 2];

            for (var di = 0; di < 2; di++)
            {
                for (var dj = 0; dj < 2; dj++)
                {
                    for (var dk = 0; dk < 2; dk++)
                    {
                        c[di, dj, dk] = ranVec[
                            permX[(i + di) & 255] ^
                            permY[(j + dj) & 255] ^
                            permZ[(k + dk) & 255]
                        ];
                    }
                }
            }

            return PerlinInterp(c, u, v, w);
        }

        public double Turb(Vec3 p, int depth = 7)
        {
            var accum = 0.0;
            var tmp = p;
            var weight = 1.0;

            for (int i = 0; i < depth; i++)
            {
                accum += weight * Noise(tmp);
                weight *= 0.5;
                tmp *= 2;
            }

            return Math.Abs(accum);
        }

        private double PerlinInterp(Vec3[,,] c, double u, double v, double w)
        {
            var uu = u * u * (3 - 2 * u);
            var vv = v * v * (3 - 2 * v);
            var ww = w * w * (3 - 2 * w);
            var accum = 0.0;

            for (var i = 0; i < 2; i++)
            {
                for (var j = 0; j < 2; j++)
                {
                    for (var k = 0; k < 2; k++)
                    {
                        var weightV = new Vec3(u - i, v - j, w - k);
                        accum += (i * u + (1 - i) * (1 - u)) *
                                 (j * v + (1 - j) * (1 - v)) *
                                 (k * w + (1 - k) * (1 - w)) *
                                 Vec3.Dot(c[i, j, k], weightV);
                    }
                }
            }

            return accum;
        }
        
        private int[] GeneratePerm()
        {
            var toReturn = new int[PointCount];
            for (var i = 0; i < PointCount; i++)
            {
                toReturn[i] = i;
            }

            Permute(toReturn, PointCount);
            return toReturn;
        }

        private void Permute(int[] p, int n)
        {
            for (int i = n - 1; i > 0; i--)
            {
                var target = MathUtils.RandomInt(0, i + 1);
                int tmp = p[i];
                p[i] = p[target];
                p[target] = tmp;
            }
        }
    }
}