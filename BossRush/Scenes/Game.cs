// ================================================================================
// File : Game.cs
// Project name : BossRush
// Project members :
// - Florian Duruz, Mathieu Rabot, Raphaël Perret
// ================================================================================

using BossRush.Enemy;
using BossRush.Entities;
using BossRush.Managers;
using BossRush.Particles;
using BossRush.UIComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Scenes;

/**
 * @brief The Game scene of the BossRush game.
 * @details The Game scene of the BossRush game. This scene handles the main gameplay loop, including player actions, enemy interactions, and rendering of game elements.
 * @param sm The SceneManager instance to manage scene transitions and actions.
 */
public class Game(SceneManager sm) : Scene(sm)
{ 
    
    private readonly Color BACKGROUND_COLOR = new (160, 200, 120);
    
    /**
     * @brief Loads the game resources.
     */
    protected override void Load()
    { }

    /**
     * @brief Activates the game scene.
     * @details This method initializes the ParticleSystem, ProjectileSystem, Player, HealthBar, and GameManager.
     */
    public override void Activate()
    {
        ParticleSystem.Initialize();
        ProjectileSystem.Initialize();
        Player.Initialize(new Vector2(Globals.ScreenSize().X/2,Globals.ScreenSize().Y/2));
        HealthBar.MaxHealth = Player.Instance.MaxHealth;
        HealthBar.OldHealth = Player.Instance.CurrentHealth;
        GameManager.Initialize();
    }
    
    /**
     * @brief Draws the game scene.
     * @param spriteBatch The SpriteBatch used for drawing the scene.
     * @details This method draws the background, player, enemies, projectiles, particles, health bar, and game manager UI.
     */
    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(Globals.WhitePixel,
            new Rectangle(0, 0, Globals.ScreenSize().X, Globals.ScreenSize().Y),
            BACKGROUND_COLOR);
        spriteBatch.End();
        spriteBatch.Begin();
        ProjectileSystem.Instance.Draw(spriteBatch);
        ParticleSystem.Instance.Draw(spriteBatch);
        Player.Instance.Draw(spriteBatch);
        EnemySystem.Instance.Draw(spriteBatch);
        spriteBatch.End();
        
        HealthBar.Draw(spriteBatch);
        GameManager.Draw(spriteBatch);
        
    }

    /**
     * @brief Updates the game scene.
     * @param gameTime The current game time.
     * @details This method updates the game state, including spawning particles, updating projectiles, player, enemies, health bar, and game manager.
     */
    public override void Update(GameTime gameTime)
    {
        ProjectileSystem.Instance.Update(gameTime);
        ParticleSystem.Instance.Update(gameTime);
        Player.Instance.Update(gameTime);
        EnemySystem.Instance.Update(gameTime, Player.Instance.Position);
        HealthBar.Update(Player.Instance.CurrentHealth, Player.Instance.MaxHealth);
        GameManager.Update(gameTime);
    }
}