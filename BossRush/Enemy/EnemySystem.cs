using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Enemy;

public class EnemySystem
{
    private static EnemySystem instance;
    public static EnemySystem Instance => instance ??= new EnemySystem();
    
    private Dictionary<Vector2, List<Enemy>> spatialGrid = new();
    
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
        UpdatePositions();
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
                Vector2 meleePos = positions[i] + new Vector2(0, Globals.ScreenSize().Y + 32);
                Register(EnemyDirector.CreateRangeEnemyLevel(level, meleePos));
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
    
    public void UpdatePositions(float cellSize = 32f)
    {
        spatialGrid.Clear();

        // 1. Remplir la grille
        foreach (Enemy enemy in Enemies)
        {
            Vector2 cellKey = new Vector2(
                (int)(enemy.Position.X / cellSize),
                (int)(enemy.Position.Y / cellSize));

            if (!spatialGrid.ContainsKey(cellKey))
                spatialGrid[cellKey] = new List<Enemy>();
            spatialGrid[cellKey].Add(enemy);
        }
    }

    public List<Enemy> GetNearbyEnemies(Vector2 pos, float cellSize = 32)
    {
        List<Enemy> result = new List<Enemy>();
        Vector2 centerCell = new Vector2((int)(pos.X / cellSize), (int)(pos.Y / cellSize));

        // Vérifie les 9 cellules autour (3x3)
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2 cellKey = new Vector2(centerCell.X + x, centerCell.Y + y);
                if (spatialGrid.TryGetValue(cellKey, out List<Enemy> enemies))
                    result.AddRange(enemies);
            }
        }
        return result;
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