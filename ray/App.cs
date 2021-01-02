﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

            var aspect = 1;
            //var generator = new RandomWorld();
            //var generator = new TwoSpheres();
            //var generator = new PerlinTest();
            //var generator = new EarthTest();
            //var generator = new SimpleLightTest();
            //var generator = new RectTest();
            var generator = new CornellBox();
            
            var imageWidth = 720;
            var imageHeight = (int) (imageWidth / aspect);
            var cam = generator.GetCamera(aspect);
            var objects = generator.Generate();
            var world = new BvhNode(objects, 0, objects.Count, 0, 1);

            var start = DateTime.Now;
            var image = new Image(imageWidth, imageHeight);
            var samplesPerPixel = 5000;
            var maxDepth = 50;
            var linesRemaining = imageHeight;
            //for(var y = 0; y < image.Height; y++) 
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
                        pixelColor += ray.GetColor(world, maxDepth, cam.Background);
                    }

                    pixelColor /= samplesPerPixel;
                    image.SetPixel(x, y, pixelColor.GammaCorrected());
                }

                Interlocked.Decrement(ref linesRemaining);
                Console.WriteLine($"Lines remaining: {linesRemaining}");
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