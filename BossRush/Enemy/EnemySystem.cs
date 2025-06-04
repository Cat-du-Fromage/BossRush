using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Enemy;

public class EnemySystem
{
    private static EnemySystem instance;
    public static EnemySystem Instance => instance ??= new EnemySystem();
//╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
//║                                               ◆◆◆◆◆◆ PROPERTIES ◆◆◆◆◆◆                                             ║
//╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝
    public List<Enemy> Enemies { get; private set; } = new();
    
//╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
//║                                             ◆◆◆◆◆◆ CONSTRUCTOR ◆◆◆◆◆◆                                              ║
//╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝

    public EnemySystem()
    {
        instance = this;
    }
    
//╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
//║                                           ◆◆◆◆◆◆ MONOGAME EVENTS ◆◆◆◆◆◆                                            ║
//╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝
    public void Update(GameTime gameTime, Vector2 target)
    {
        for (int i = Enemies.Count - 1; i > -1; i--)
        {
            if (Enemies[i].CurrentHealth <= 0)
            {
                Enemies.RemoveAt(i);
            }
            else
            {
                Enemies[i].OnUpdate(gameTime, target);
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (Enemy enemy in Enemies)
        {
            enemy.Draw(spriteBatch);
        }
    }
    
//╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
//║                                            ◆◆◆◆◆◆ CLASS METHODS ◆◆◆◆◆◆                                             ║
//╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝

    public void Register(Enemy enemy)
    {
        if (Enemies.Contains(enemy)) return;
        Enemies.Add(enemy);
    }

    public void Unregister(Enemy enemy)
    {
        Enemies.Remove(enemy);
    }

    public void GetWave(int level)
    {
        LevelComposition levelComposition = new LevelComposition(level);
        if (levelComposition.Count == 1)
        {
            Vector2 position = new Vector2(Globals.ScreenSize().X, Globals.ScreenSize().Y / 2);
            Register(EnemyDirector.CreateBossEnemyLevel(level, position));
        }
        else
        {
            //Melee
            Vector2[] positions = GetPositions(levelComposition.Count);
            for (int i = 0; i < levelComposition.MeleeCount; i++)
            {
                Register(EnemyDirector.CreateMeleeEnemyLevel(level, positions[i]));
            }
            //Range
            for (int i = levelComposition.MeleeCount; i < positions.Length; i++)
            {
                Register(EnemyDirector.CreateRangeEnemyLevel(level, positions[i]));
            }
        }
    }

    private Vector2[] GetPositions(int enemyCount)
    {
        Vector2[] positions = new Vector2[enemyCount];
        int maxHeight = Globals.ScreenSize().Y / 20;
        int startX = Globals.ScreenSize().X;
        int startY = 0;
        for (int i = 0; i < enemyCount; i++)
        {
            int y = i / maxHeight;
            int x = i - (y * maxHeight);
            positions[i] = new Vector2(startX + y * 16, startY + x * 16);
        }
        return positions;
    }
    
    public void TestBuildEnemies()
    {
        for (int i = 0; i < 16; i++)
        {
            int y = i / 4;
            int x = i - y * 4;
            Vector2 pos = new Vector2(x, y) * 32 + new Vector2(32,32);
            Register(EnemyDirector.CreateBasicEnemy(pos));
        }
    }
}