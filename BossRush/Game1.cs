using BossRush.Enemy;
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

    private EnemySystem enemySystem = new();

    public Game1()
    {
        Globals.GameInstance = this;
        graphics = new GraphicsDeviceManager(this);
        graphics.PreferredBackBufferWidth = 1280;
        graphics.PreferredBackBufferHeight = 720;
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
        
        //TODO remplacer target par Le joueur
        enemySystem.Update(gameTime, Mouse.GetState().Position.ToVector2());
        
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

    private void TestBuildEnemies()
    {
        for (int i = 0; i < 16; i++)
        {
            int y = i / 4;
            int x = i - y * 4;
            Vector2 pos = new Vector2(x, y) * 32 + new Vector2(32,32);
            enemySystem.Register(EnemyDirector.CreateBasicEnemy(pos, this));
        }
    }
}
