using System;

namespace BossRush.Enemy;

/**
 * Calculates stat multipliers for game entities based on their level.
 * Provides exponential scaling for various combat attributes to create progressive difficulty.
 */
public struct StatsMultiplicator
{
    /**
     * The entity level these multipliers are calculated for
     */
    public readonly int Level;

    /**
     * Health multiplier (15% increase per level)
     * @return Exponential health scaling factor
     */
    public int Health  => (int)Math.Pow(1.15, Level - 1);

    /**
     * Damage multiplier (8% increase per level)
     * @return Linear damage scaling factor
     */
    public int Damage  => (int)(1 + (Level - 1) * 0.08);

    /**
     * Speed multiplier (0.02 increase per level)
     * @return Linear speed scaling with base of 3
     */
    public float Speed => 3 + (Level - 1) * 0.02f;

    /**
     * Defense multiplier (logarithmic scaling)
     * @return Defense value based on log10 of level
     */
    public int Defense => (int)(Math.Log10(Level + 9));

    /**
     * Range multiplier (3% increase per level)
     * @return Linear range scaling with base of 1
     */
    public float Range => 1 + ((Level - 1) * 1.03f);

    /**
     * Attack cooldown reducer (2% faster per level)
     * @return Cooldown multiplier (values < 1 = faster attacks)
     */
    public float AttackCooldown => (float)Math.Pow(0.98f, Level - 1);

    /**
     * Creates a new stat multiplier calculator
     * @param level The entity level to calculate for
     */
    public StatsMultiplicator(int level)
    {
        Level = level;
    } 
}