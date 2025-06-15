using BossRush.Entities;
using Microsoft.Xna.Framework;

namespace BossRush.Enemy;

/**
 * Enemy Builder Directory
 * Provides predefined enemy templates (melee, ranged, boss) with progressive difficulty scaling.
 */
public static class EnemyDirector
{
    /**
     * Creates a melee enemy scaled to specified level
     * @param level The difficulty level to scale stats
     * @param position Spawn position in world coordinates
     * @return Configured melee enemy instance
     */
    public static Enemy CreateMeleeEnemyLevel(int level, Vector2 position)
    {
        return CreateBasicMeleeEnemy(position, new StatsMultiplicator(level));
    }
    
    /**
     * Creates a ranged enemy scaled to specified level
     * @param level The difficulty level to scale stats
     * @param position Spawn position in world coordinates
     * @return Configured ranged enemy instance
     */
    public static Enemy CreateRangeEnemyLevel(int level, Vector2 position)
    {
        return CreateBasicRangeEnemy(position, new StatsMultiplicator(level));
    }
    
    /**
     * Creates a boss enemy scaled to specified level
     * @param level The difficulty level to scale stats
     * @param position Spawn position in world coordinates
     * @return Configured boss enemy instance
     */
    public static Enemy CreateBossEnemyLevel(int level, Vector2 position)
    {
        return CreateBasicBossEnemy(position, new StatsMultiplicator(level));
    }
    
    /**
     * Template for basic melee enemy configuration
     * @param position Spawn position in world coordinates
     * @param multiplicator Stat scaling calculator
     * @return Configured melee enemy
     */
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
    
    /**
     * Template for basic ranged enemy configuration
     * @param position Spawn position in world coordinates
     * @param multiplicator Stat scaling calculator
     * @return Configured ranged enemy
     */
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
    
    /**
     * Template for basic boss enemy configuration
     * @param position Spawn position in world coordinates
     * @param multiplicator Stat scaling calculator
     * @return Configured boss enemy
     */
    private static Enemy CreateBasicBossEnemy(Vector2 position, StatsMultiplicator multiplicator)
    {
        return new Enemy.Builder(position, Vector2.Zero)
            .WithName("BasicBossEnemy")
            .WithColor(Color.Navy)
            .IsMelee(false)
            .WithSize(64)
            .WithDamage(16 * multiplicator.Damage)
            .WithHealth(100 * multiplicator.Health)
            .WithMoveSpeed(50f * multiplicator.Speed)
            .WithRange(500 * multiplicator.Range)
            .WithAttackCooldown(multiplicator.AttackCooldown)
            .WithAbility(new BaseAttack().Apply(new Arrow()))
            .Build();
    }
}