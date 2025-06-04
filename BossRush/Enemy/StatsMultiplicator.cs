using System;

namespace BossRush.Enemy;

public struct StatsMultiplicator
{
    public readonly int Level;
    public int Health  => (int)Math.Pow(1.15, Level - 1);
    public int Damage  => (int)(1 + (Level - 1) * 0.08);
    public float Speed => 1 + (Level - 1) * 0.02f;
    public int Defense => (int)(Math.Log10(Level + 9));
    public float Range => 1 + ((Level - 1) * 1.03f);

    public StatsMultiplicator(int level)
    {
        Level = level;
    }
}