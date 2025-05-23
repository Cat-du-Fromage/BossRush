using System;
using BossRush.Entities;
using BossRush.Managers;
using BossRush.Scenes.UIComponents;
using BossRush.UIComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Scenes;

public class Game(SceneManager sm) : Scene(sm)
{ 
    
    private readonly Color BACKGROUND_COLOR = new (160, 200, 120);
    protected override void Load()
    {
        
    }

    public override void Activate()
    {
        ProjectileSystem.Initialize();
        Player.Initialize(new Vector2(Globals.ScreenSize().X/2,Globals.ScreenSize().Y/2));
        HealthBar.MaxHealth = Player.Instance.MaxHealth;
        HealthBar.OldHealth = Player.Instance.CurrentHealth;
        GameManager.Initialize();
    }
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(Globals.WhitePixel,
            new Rectangle(0, 0, Globals.ScreenSize().X, Globals.ScreenSize().Y),
            BACKGROUND_COLOR);
        spriteBatch.End();
        spriteBatch.Begin();
        ProjectileSystem.Instance.Draw(spriteBatch);
        Player.Instance.Draw(spriteBatch);
        spriteBatch.End();
        
        HealthBar.Draw(spriteBatch);
        
        GameManager.Draw(spriteBatch);
        
    }

    public override void Update(GameTime gameTime)
    {
        ProjectileSystem.Instance.Update(gameTime);
        Player.Instance.Update(gameTime);
        HealthBar.Update(Player.Instance.CurrentHealth, Player.Instance.MaxHealth);
        GameManager.Update(gameTime);
    }
}