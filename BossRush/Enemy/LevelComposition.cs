using System;

namespace BossRush.Enemy;

/**
 * Represents the composition of enemies for a specific game level.
 * Determines the number and type of enemies based on level difficulty,
 * with special boss levels every 5th level.
 */
public class LevelComposition
{
    /**
     * The difficulty level this composition represents
     */
    public int Level { get; }

    /**
     * Number of melee enemies in this level
     */
    public int MeleeCount { get; }

    /**
     * Number of ranged enemies in this level
     */
    public int RangedCount { get; }

    /**
     * Indicates if this level contains a boss enemy
     * @return True if level is a multiple of 5, false otherwise
     */
    public bool HasBoss => Level % 5 == 0;

    /**
     * Total number of enemies in this level
     * @return 1 for boss levels, otherwise melee + ranged count
     */
    public int Count => HasBoss ? 1 : MeleeCount + RangedCount;
    
    /**
     * Creates a new level composition
     * @param level The difficulty level to generate composition for
     */
    public LevelComposition(int level)
    {
        Level = level;
        
        if (HasBoss)
        {
            // Boss level - only 1 boss enemy
            MeleeCount = 0;
            RangedCount = 0;
        }
        else
        {
            // Base formula with moderated exponential progression
            double baseEnemies = 4 + level;
            
            // Random distribution between melee/ranged (40-60%)
            Random rand = new Random(level); // Level-based seed for consistency
            double meleeRatio = 0.4 + (rand.NextDouble() * 0.2); // Between 40% and 60%

            MeleeCount = (int)Math.Round(baseEnemies * meleeRatio);
            RangedCount = (int)Math.Round(baseEnemies * (1 - meleeRatio));

            // Minimum of 1 enemy per type
            MeleeCount = Math.Max(1, MeleeCount);
            RangedCount = Math.Max(1, RangedCount);
        }
    }
}