using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.UIComponents;

public static class HealthBar
{
    public static int OldHealth { get; set; }
    public static int MaxHealth { get; set; }
    private static int Width => (int)(Globals.ScreenSize().X * 0.2f);
    private static int Height => (int)(Globals.ScreenSize().Y * 0.05f);

    public static void Update(int newHealth, int newMaxHealth)
    {
        MaxHealth = newMaxHealth;
        OldHealth = newHealth;
    }

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