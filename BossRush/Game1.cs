using BossRush.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BossRush;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private BasicEffect _basicEffect;

    public static Game Instance { get; private set; }

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.IsFullScreen = true;
        
        int ScreenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        int ScreenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

        _graphics.PreferredBackBufferWidth = ScreenWidth;
        _graphics.PreferredBackBufferHeight = ScreenHeight;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Instance = this;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        Projectile.Builder builder = new Projectile.Builder().SetGame(this)
            .SetPosition(new Vector2(300, 200))
            .SetVelocity(new Vector2(25,25))
            .SetMaxSpeed(100)
            .SetMaxAcceleration(2)
            .SetDirect((projectile =>
            {
                Vector2 center = new Vector2(350,180);
                Vector2 pc = center - projectile.Position;
                
                return pc;
            }));

        Player player = new Player(new Vector2(200, 300), this);
        Components.Add(player);
        
        Projectile proj1 = builder.Build();
        Components.Add(proj1);
        Projectile.Director.MakeGuided(builder)
            .SetTarget(player)
            .SetPosition(new Vector2(300, 0))
            .SetVelocity(new Vector2(0,0))
            .SetMaxAcceleration(120)
            .SetFriction(0.3f)
            .SetMaxSpeed(100);
        
        Components.Add(builder.Build());

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        new SimpleShapes(GraphicsDevice);

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}
