using Microsoft.Xna.Framework;

namespace BossRush.Enemy;

public static class EnemyDirector
{
    // Archétype de base
    public static Enemy CreateBasicEnemy(Vector2 position)
    {
        return new Enemy.Builder(position, Vector2.Zero)
            .WithName("Basic Enemy")
            .WithHealth(50)
            .WithMoveSpeed(40f)
            .WithRange(100f)
            .Build();
    }
}