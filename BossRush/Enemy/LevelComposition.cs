using System;

namespace BossRush.Enemy;

public class LevelComposition
{
    public int Level { get; }
    public int MeleeCount { get; }
    public int RangedCount { get; }
    public bool HasBoss => Level % 5 == 0;

    public int Count => HasBoss ? 1 : MeleeCount + RangedCount;
    
    public LevelComposition(int level)
    {
        Level = level;
        
        if (HasBoss)
        {
            // Niveau de boss - uniquement 1 boss
            MeleeCount = 0;
            RangedCount = 0;
        }
        else
        {
            // Formule de base avec progression exponentielle modérée
            double baseEnemies = 4 + level;
            
            // Répartition aléatoire entre melee/ranged (40-60%)
            Random rand = new Random(level); // Seed basée sur le niveau pour cohérence
            double meleeRatio = 0.4 + (rand.NextDouble() * 0.2); // Entre 40% et 60%

            MeleeCount = (int)Math.Round(baseEnemies * meleeRatio);
            RangedCount = (int)Math.Round(baseEnemies * (1 - meleeRatio));

            // Minimum de 1 ennemi par type
            MeleeCount = Math.Max(1, MeleeCount);
            RangedCount = Math.Max(1, RangedCount);
        }
    }
}