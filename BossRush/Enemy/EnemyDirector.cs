using BossRush.Entities;
using Microsoft.Xna.Framework;

namespace BossRush.Enemy;

public static class EnemyDirector
{

    public static Enemy CreateMeleeEnemyLevel(int level, Vector2 position)
    {
        return CreateBasicMeleeEnemy(position, new StatsMultiplicator(level));
    }
    
    public static Enemy CreateRangeEnemyLevel(int level, Vector2 position)
    {
        return CreateBasicRangeEnemy(position, new StatsMultiplicator(level));
    }
    
    public static Enemy CreateBossEnemyLevel(int level, Vector2 position)
    {
        return CreateBasicBossEnemy(position, new StatsMultiplicator(level));
    }
    
    private static Enemy CreateBasicMeleeEnemy(Vector2 position, StatsMultiplicator multiplicator)
    {
        return new Enemy.Builder(position, Vector2.Zero)
            .WithName("BasicMeleeEnemy")
            .WithSize(16)
            .WithHealth(10 * multiplicator.Health)
            .WithMoveSpeed(40f * multiplicator.Speed)
            .WithRange(0)
            .Build();
    }
    
    private static Enemy CreateBasicRangeEnemy(Vector2 position, StatsMultiplicator multiplicator)
    {
        return new Enemy.Builder(position, Vector2.Zero)
            .WithName("BasicRangeEnemy")
            .WithSize(16)
            .WithHealth(10 * multiplicator.Health)
            .WithMoveSpeed(40f * multiplicator.Speed)
            .WithRange(10 * multiplicator.Range)
            .Build();
    }
    
    private static Enemy CreateBasicBossEnemy(Vector2 position, StatsMultiplicator multiplicator)
    {
        return new Enemy.Builder(position, Vector2.Zero)
            .WithName("BasicBossEnemy")
            .WithSize(32)
            .WithHealth(100 * multiplicator.Level * multiplicator.Health)
            .WithMoveSpeed(50f * multiplicator.Speed)
            .WithRange(10 * multiplicator.Range)
            .Build();
    }
    
    // Archétype de base
    public static Enemy CreateBasicEnemy(Vector2 position)
    {
        return new Enemy.Builder(position, Vector2.Zero)
            .WithName("Basic Enemy")
            .WithSize(16)
            .WithHealth(50)
            .WithMoveSpeed(40f)
            .WithRange(100f)
            .Build();
    }
}