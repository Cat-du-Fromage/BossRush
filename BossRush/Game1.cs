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
        Globals.Content = Content;
        Globals.LoadContent();
        
        sceneManager = new SceneManager();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        sceneManager.Update(gameTime);
        // TODO: Add your update logic here
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        sceneManager.Draw(new SpriteBatch(GraphicsDevice));

        base.Draw(gameTime);
    }
}
