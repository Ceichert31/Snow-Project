using System.Collections.Generic;

namespace OrthoSnowSplat
{
    public static class SnowObstacleRegistry
    {
        private static readonly List<SnowObstacle> obstacles = new();

        public static IReadOnlyList<SnowObstacle> Obstacles => obstacles;

        public static void Register(SnowObstacle o)
        {
            if (o != null && !obstacles.Contains(o)) obstacles.Add(o);
        }

        public static void Deregister(SnowObstacle o)
        {
            obstacles.Remove(o);
        }
    }
}
