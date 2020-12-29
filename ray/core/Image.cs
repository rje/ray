using System.IO;
using System.Text;

namespace ray.core
{
    public class Image
    {
        public int Width;
        public int Height;
        public Vec3[] Pixels;

        public static Image GenerateTestImage()
        {
            var toReturn = new Image(256, 256);

            for (int y = toReturn.Height - 1; y >= 0; y--)
            {
                for (int x = 0; x < toReturn.Width; x++)
                {
                    var r = x / (double) (toReturn.Width - 1);
                    var g = y / (double) (toReturn.Height - 1);
                    var b = 0.25f;
                    
                    toReturn.SetPixel(x, y, new Vec3(r, g, b));
                }
            }

            return toReturn;
        }

        public Image(int w, int h)
        {
            Width = w;
            Height = h;
            Pixels = new Vec3[w * h];
        }

        public Vec3 GetPixel(int x, int y)
        {
            y = Height - 1 - y;
            return Pixels[y * Width + x];
        }

        public void SetPixel(int x, int y, Vec3 val)
        {
            y = Height - 1 - y;
            Pixels[y * Width + x] = val;
        }

        public void WriteToPpm(string path)
        {
            var sb = new StringBuilder();
            sb.AppendLine("P3");
            sb.AppendLine($"{Width} {Height}");
            sb.AppendLine("255");
            for (var y = Height - 1; y >= 0; y--)
            {
                for (var x = 0; x < Width; x++)
                {
                    var toWrite = GetPixel(x, y);
                    var r = (int) (256 * MathUtils.Clamp(toWrite.r, 0.0, 0.999));
                    var g = (int) (256 * MathUtils.Clamp(toWrite.g, 0.0, 0.999));
                    var b = (int) (256 * MathUtils.Clamp(toWrite.b, 0.0, 0.999));
                    sb.Append($"{r} {g} {b}");
                    if (x < Width - 1)
                    {
                        sb.Append(' ');
                    }
                }

                sb.AppendLine();
            }
            File.WriteAllText(path, sb.ToString());
        }
    }
}