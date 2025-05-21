using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
// ReSharper disable All

namespace BossRush;

public class Game1 : Game
{
    private BasicEffect basicEffect;
    private GraphicsDeviceManager graphics;
    private SceneManager sceneManager;

    public Game1()
    {
        Globals.GameInstance = this;
        graphics = new GraphicsDeviceManager(this);
        
        int screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        int screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        
        graphics.PreferredBackBufferWidth = screenWidth;
        graphics.PreferredBackBufferHeight = screenHeight;
        graphics.ApplyChanges();
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }
    
    protected override void LoadContent()
    {
        Globals.GraphicsDevice = GraphicsDevice;
        Globals.SpriteBatch = new SpriteBatch(GraphicsDevice);
        Globals.Content = Content;
        new SimpleShapes(Globals.GraphicsDevice);
        Globals.LoadContent();
        
        sceneManager = new SceneManager(new GameManager());
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        sceneManager.Update();
        // TODO: Add your update logic here
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        RenderTarget2D frame = sceneManager.GetFrame();
        
        Globals.SpriteBatch.Begin();
        Globals.SpriteBatch.Draw(frame, new Rectangle(0, 0, Globals.ScreenSize.X, Globals.ScreenSize.Y), Color.White);
        Globals.SpriteBatch.End();

        base.Draw(gameTime);
    }
}
