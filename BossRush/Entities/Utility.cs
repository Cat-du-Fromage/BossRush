using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace BossRush.Entities;

static class Utility
{
    public static Vector2 SetLength(Vector2 vector, float length)
    {
        if (vector.LengthSquared() == 0)
            return vector;
        vector.Normalize(); 
        return vector * length;
    }

    public static Vector2 Projection(Vector2 v, Vector2 n)
    {
        return Vector2.Dot(v, n) / n.LengthSquared() * n;
    }

    public static List<EntityBase> Merge (
        IReadOnlyCollection<EntityBase> c1,
        IReadOnlyCollection<EntityBase> c2)
    {
        List<EntityBase> result = [];
        result.AddRange(c1);
        result.AddRange(c2);
        return result;
    }
}