using System;
using System.IO;
using ray.core;
using StbImageSharp;

namespace ray.texture
{
    public class ImageTexture : ITexture
    {
        public ImageResult Image;

        public ImageTexture(string path)
        {
            try
            {
                using var stream = File.OpenRead(path);
                Image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlue);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public Vec3 Value(double u, double v, Vec3 p)
        {
            if (Image == null)
            {
                return new Vec3(1, 0, 1);
            }

            u = MathUtils.Clamp(u, 0, 1);
            v = 1.0 - MathUtils.Clamp(v, 0, 1);

            var i = (int) (u * Image.Width);
            var j = (int) (v * Image.Height);

            if (i >= Image.Width) i = Image.Width - 1;
            if (j >= Image.Height) j = Image.Height - 1;

            var idx = i * 3 + j * Image.Width * 3;
            var colorScale = 1 / 255.0;
            var r = Image.Data[idx + 0] * colorScale;
            var g = Image.Data[idx + 1] * colorScale;
            var b = Image.Data[idx + 2] * colorScale;

            return new Vec3(r, g, b);
        }
    }
}