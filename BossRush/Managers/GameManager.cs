using System.Collections.Generic;
using System.Linq;
using BossRush.Enemy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace BossRush.Managers;

public static class GameManager
{
    private const int MAX_WAVES = 10;
    private static int currentWave;
    private static List<Enemy.Enemy> remainingEnemies = [];

    public static void Initialize()
    {
        currentWave = 1;
        //Todo remainingEnemies = EnemySystem.Instance.GetWave(currentWave);
    }
    
    public static void Update(GameTime gameTime)
    {
        if (IsWaveComplete())
        {
            NextWave();
        }
    }
    
    public static void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        Vector2 textSize = Globals.Font.MeasureString("Wave : " + currentWave + "/" + MAX_WAVES);
        Vector2 textPosition = new Vector2(Globals.ScreenSize().X - 10 - textSize.X, 10);
        spriteBatch.DrawString(Globals.Font, "Wave : " + currentWave + "/" + MAX_WAVES, textPosition, Color.White);
        spriteBatch.End();
    }
    
    private static void NextWave()
    {
        if (currentWave < MAX_WAVES)
        {
            //Todo remainingEnemies = EnemySystem.Instance.GetWave(++currentWave);
        }
    }
    
    private static bool IsWaveComplete()
    {
        return remainingEnemies.All(enemy => !enemy.IsAlive());
    }
}