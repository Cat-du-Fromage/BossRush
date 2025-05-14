using System.Drawing;
using BossRush.Scenes;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush;

public static class Globals
{
    public static Point ScreenSize { get; set; } = new(800, 600);
    public static SpriteBatch SpriteBatch { get; set; }
    public static GraphicsDevice GraphicsDevice { get; set; }
    public static ContentManager Content { get; set; }
    
    public static Game1 GameInstance { get; set; }

    public static SpriteFont Font { get; private set; }
    
    public static void LoadContent()
    {
        Font = Content.Load<SpriteFont>("default");
    }

    public static RenderTarget2D GetNewRenderTarget()
    {
        return new RenderTarget2D(GraphicsDevice, ScreenSize.X, ScreenSize.Y);
    }
}