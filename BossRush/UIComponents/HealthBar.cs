// ================================================================================
// File : HealthBar.cs
// Project name : BossRush
// Project members :
// - Florian Duruz, Mathieu Rabot, Raphaël Perret
// ================================================================================
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.UIComponents;

/**
 * @brief Represents a health bar UI component.
 * @details This class manages the health bar's state, including the current health and maximum health.
 */
public static class HealthBar
{
    public static int OldHealth { get; set; }
    public static int MaxHealth { get; set; }
    private static int Width => (int)(Globals.ScreenSize().X * 0.2f);
    private static int Height => (int)(Globals.ScreenSize().Y * 0.05f);

    /**
     * @brief Initializes the health bar with default values.
     * @details Sets the initial health and maximum health for the health bar.
     * @param newHealth The current health value.
     * @param newMaxHealth The maximum health value.
     */
    public static void Update(int newHealth, int newMaxHealth)
    {
        MaxHealth = newMaxHealth;
        OldHealth = newHealth;
    }

    /**
     * @brief Draws the health bar on the screen.
     * @param spriteBatch The SpriteBatch used for drawing the health bar.
     * @details This method draws the health bar background, border, and current health level.
     */
    public static void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(Globals.WhitePixel,
            new Rectangle(10, 10, Width, Height),
            Color.Black);
        spriteBatch.Draw(Globals.WhitePixel,
            new Rectangle(12, 12, Width - 4, Height - 4),
            Color.White);
        spriteBatch.Draw(Globals.WhitePixel,
            new Rectangle(12, 12, (int)((Width - 4) * ((float)OldHealth / MaxHealth)), Height - 4),
            Color.Red); 
        spriteBatch.End();
    }
}