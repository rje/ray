using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ray.core;
using ray.hittables;
using ray.texture;
using ray.worlds;

namespace ray
{
    internal static class App
    {
        public static void Main(string[] args)
        {
            var aspect = 16.0 / 9.0;
            var imageWidth = 1280;
            var imageHeight = (int) (imageWidth / aspect);
            
            //var generator = new RandomWorld();
            //var generator = new TwoSpheres();
            var generator = new PerlinTest();
            
            var cam = generator.GetCamera(aspect);
            var objects = generator.Generate();
            var world = new BvhNode(objects, 0, objects.Count, 0, 1);

            var start = DateTime.Now;
            var image = new Image(imageWidth, imageHeight);
            var samplesPerPixel = 100;
            var maxDepth = 50;
            ThreadPool.SetMinThreads(12, 12);
            ThreadPool.SetMaxThreads(12, 12);
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