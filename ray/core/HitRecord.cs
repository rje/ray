namespace ray.core
{
    public struct HitRecord
    {
        public Vec3 Point;
        public Vec3 Normal;
        public double U;
        public double V;
        public double T;
        public bool IsFrontFace;
        public IMaterial Material;

        public void SetFaceNormal(Ray r, Vec3 outwardNormal)
        {
            IsFrontFace = Vec3.Dot(r.Dir, outwardNormal) < 0;
            Normal = IsFrontFace ? outwardNormal : -outwardNormal;
        }
    }
}