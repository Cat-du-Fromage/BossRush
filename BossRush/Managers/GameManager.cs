// ================================================================================
// File : GameManager.cs
// Project name : BossRush
// Project members :
// - Florian Duruz, Mathieu Rabot, Raphaël Perret
// ================================================================================
using BossRush.Enemy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace BossRush.Managers;

/**
 * @brief Manages the game state, including waves and enemy counts.
 * @details This class is responsible for tracking the current wave of enemies, checking if a wave is complete, and transitioning to the next wave.
 */
public static class GameManager
{
    private const int MAX_WAVES = 10;
    private static int currentWave;

    /**
     * @brief Initializes the GameManager.
     * @details Resets the current wave to 0 at the start of the game.
     */
    public static void Initialize()
    {
        currentWave = 0;
    }
    
    /**
     * @brief Gets the current wave number.
     * @return The current wave number.
     */
    public static void Update(GameTime gameTime)
    {
        if (IsWaveComplete())
        {
            NextWave();
        }
    }
    
    /**
     * @brief Draws the current wave and enemy count on the screen.
     * @param spriteBatch The SpriteBatch used for drawing.
     * @details This method draws the current wave number and the number of remaining enemies on the screen.
     */
    public static void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        Vector2 textSize = Globals.Font.MeasureString("Wave : " + currentWave + "/" + MAX_WAVES);
        Vector2 textPosition = new Vector2(Globals.ScreenSize().X - 10 - textSize.X, 10);
        spriteBatch.DrawString(Globals.Font, "Wave : " + currentWave + "/" + MAX_WAVES, textPosition, Color.White);

        DisplayEnemyCount(spriteBatch);
        spriteBatch.End();
    }

    /**
     * @brief Displays the count of remaining enemies on the screen.
     * @param spriteBatch The SpriteBatch used for drawing.
     * @details This method draws the number of remaining enemies at a fixed position on the screen.
     */
    private static void DisplayEnemyCount(SpriteBatch spriteBatch)
    {
        Vector2 textSize = Globals.Font.MeasureString("Enemy Remaining : " + EnemySystem.Instance.Enemies.Count);
        Vector2 textPosition = new Vector2(Globals.ScreenSize().X - 10 - textSize.X, 50);
        spriteBatch.DrawString(Globals.Font, "Num Enemies : " + EnemySystem.Instance.Enemies.Count, textPosition, Color.White);
    }
    
    /**
     * @brief Advances to the next wave of enemies.
     * @details This method increments the current wave number and retrieves the next wave of enemies from the EnemySystem.
     */
    private static void NextWave()
    {
        //if (currentWave >= MAX_WAVES) return;
        EnemySystem.Instance.GetWave(++currentWave);
    }
    
    /**
     * @brief Checks if the current wave is complete.
     * @return True if there are no remaining enemies, false otherwise.
     * @details This method checks if the EnemySystem has any enemies left. If not, it considers the wave complete.
     */
    private static bool IsWaveComplete()
    {
        return EnemySystem.Instance.Enemies.Count == 0;
    }
}