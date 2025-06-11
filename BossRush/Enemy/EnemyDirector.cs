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
            .WithColor(Color.Khaki)
            .IsMelee(true)
            .WithSize(40)
            .WithDamage(1 * multiplicator.Damage)
            .WithHealth(10 * multiplicator.Health)
            .WithMoveSpeed(30f * multiplicator.Speed)
            .WithRange(0)
            .WithAttackCooldown(multiplicator.AttackCooldown)
            .Build();
    }
    
    private static Enemy CreateBasicRangeEnemy(Vector2 position, StatsMultiplicator multiplicator)
    {
        return new Enemy.Builder(position, Vector2.Zero)
            .WithName("BasicRangeEnemy")
            .WithColor(Color.Purple)
            .IsMelee(false)
            .WithSize(32)
            .WithDamage(8 * multiplicator.Damage)
            .WithHealth(10 * multiplicator.Health)
            .WithMoveSpeed(40f * multiplicator.Speed)
            .WithRange(650 * multiplicator.Range)
            .WithAttackCooldown(multiplicator.AttackCooldown)
            .WithAbility(new BaseAttack().Apply(new Arrow()))
            .Build();
    }
    
    private static Enemy CreateBasicBossEnemy(Vector2 position, StatsMultiplicator multiplicator)
    {
        return new Enemy.Builder(position, Vector2.Zero)
            .WithName("BasicBossEnemy")
            .WithColor(Color.Navy)
            .IsMelee(false)
            .WithSize(64)
            .WithDamage(16 * multiplicator.Damage)
            .WithHealth(100 * multiplicator.Level * multiplicator.Health)
            .WithMoveSpeed(50f * multiplicator.Speed)
            .WithRange(500 * multiplicator.Range)
            .WithAttackCooldown(multiplicator.AttackCooldown)
            .WithAbility(new BaseAttack().Apply(new Arrow()))
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