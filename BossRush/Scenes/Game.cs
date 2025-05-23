using System;
using BossRush.Enemy;
using BossRush.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Scenes;

public class Game(GameManager gm, SceneManager sm) : Scene(gm, sm)
{ 
    
    
    protected override void Load()
    {
        EnemySystem.Instance.TestBuildEnemies();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(Globals.WhitePixel,
            new Rectangle(0, 0, Globals.ScreenSize().X, Globals.ScreenSize().Y),
            new Color(160, 200, 120));
        spriteBatch.End();
        spriteBatch.Begin();
        ProjectileSystem.Instance.Draw(spriteBatch);
        Player.Instance.Draw(spriteBatch);
        EnemySystem.Instance.Draw(spriteBatch);
        spriteBatch.End();
    }

    public override void Update(GameTime gameTime)
    {
        ProjectileSystem.Instance.Update(gameTime);
        Player.Instance.Update(gameTime);
        EnemySystem.Instance.Update(gameTime, Player.Instance.Position);
    }

    public override void Activate()
    {
        ProjectileSystem.Initialize();
        Player.Initialize(new Vector2(Globals.ScreenSize().X/2,Globals.ScreenSize().Y/2));
    }
    
    
}