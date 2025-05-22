using System.Drawing;
using BossRush.Scenes;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush;

public static class Globals
{
    public static Point ScreenSize()
    {
        return new Point(GraphicsDevice.Viewport.Width,GraphicsDevice.Viewport.Height);
    }
    public static GraphicsDevice GraphicsDevice { get; set; }
    public static ContentManager Content { get; set; }
    
    public static Game1 GameInstance { get; set; }

    public static SpriteFont Font { get; private set; }

    public static Texture2D WhitePixel { get; private set; }

    
    public static void LoadContent()
    {
        Font = Content.Load<SpriteFont>("default");
        
        WhitePixel = new Texture2D(GraphicsDevice, 1, 1);
        WhitePixel.SetData(new[] { Microsoft.Xna.Framework.Color.White });

    }

    public static RenderTarget2D GetNewRenderTarget()
    {
        return new RenderTarget2D(GraphicsDevice, ScreenSize().X, ScreenSize().Y);
    }
}