using BossRush.Enemy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BossRush;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private EnemySystem enemySystem = new();

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
        TestBuildEnemies();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        
        //TODO remplacer target par Le joueur
        enemySystem.Update(gameTime, Mouse.GetState().Position.ToVector2());
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        
        // TODO: Add your drawing code here
        enemySystem.Draw(_spriteBatch);
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
