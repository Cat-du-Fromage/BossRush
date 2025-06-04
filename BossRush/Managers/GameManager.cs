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

    public static void Initialize()
    {
        currentWave = 0;
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

        DisplayEnemyCount(spriteBatch);
        spriteBatch.End();
    }

    private static void DisplayEnemyCount(SpriteBatch spriteBatch)
    {
        Vector2 textSize = Globals.Font.MeasureString("Enemy Remaining : " + EnemySystem.Instance.Enemies.Count);
        Vector2 textPosition = new Vector2(Globals.ScreenSize().X - 10 - textSize.X, 50);
        spriteBatch.DrawString(Globals.Font, "Num Enemies : " + EnemySystem.Instance.Enemies.Count, textPosition, Color.White);
    }
    
    private static void NextWave()
    {
        //if (currentWave >= MAX_WAVES) return;
        EnemySystem.Instance.GetWave(++currentWave);
    }
    
    private static bool IsWaveComplete()
    {
        return EnemySystem.Instance.Enemies.Count == 0;
    }
}