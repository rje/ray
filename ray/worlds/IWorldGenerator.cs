using System.Collections.Generic;
using ray.core;

namespace ray.worlds
{
    public interface IWorldGenerator
    {
        List<IHittable> Generate();
        Camera GetCamera(double aspect);
    }
}