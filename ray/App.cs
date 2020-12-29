using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ray.core;

namespace ray
{
    internal static class App
    {
        public static void Main(string[] args)
        {
            var imageWidth = 1280;
            var aspect = 16.0 / 9.0;
            var imageHeight = (int) (imageWidth / aspect);
            var samplesPerPixel = 100;
            var maxDepth = 50;
            var lookFrom = new Vec3(13, 2, 3);
            var lookAt = new Vec3(0, 0, 0);
            var distToFocus = 10.0;
            var aperture = 0.1;
            var cam = new Camera(
                lookFrom,
                lookAt,
                Vec3.Up, 
                20, 
                aspect,
                aperture,
                distToFocus
                );
            var world = WorldGenerator.RandomScene();

            var start = DateTime.Now;
            var image = new Image(imageWidth, imageHeight);
            ThreadPool.SetMinThreads(12, 12);
            Parallel.For(0, image.Height, y =>
            {
                for (var x = 0; x < image.Width; x++)
                {
                    Vec3 pixelColor = Vec3.Zero;
                    for (var i = 0; i < samplesPerPixel; i++)
                    {
                        var u = (x + MathUtils.RandDouble()) / (imageWidth - 1);
                        var v = (y + MathUtils.RandDouble()) / (imageHeight - 1);
                        var ray = cam.GetRay(u, v);
                        pixelColor += ray.GetColor(world, maxDepth);
                    }

                    pixelColor /= samplesPerPixel;
                    image.SetPixel(x, y, pixelColor.GammaCorrected());
                }

                Console.WriteLine($"Finished line {y}");
            });

            var end = DateTime.Now;
            var diff = end - start;
            Console.WriteLine($"Render Time: {diff}");
            Console.WriteLine($"Num Rays: {Ray.RayCount}");
            Console.WriteLine($"Rays per second: {Ray.RayCount / diff.TotalSeconds}");

            var file = $"render-{DateTime.Now:yyyyMMdd-HH_mm_ss}.ppm";
            image.WriteToPpm(file);
        }
    }
}