// ================================================================================
// File : Scene.cs
// Project name : BossRush
// Project members :
// - Florian Duruz, Mathieu Rabot, Raphaël Perret
// ================================================================================
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

namespace BossRush.Scenes;

/**
 * @brief Base class for all scenes in the game.
 * @details This class provides a structure for scenes, including methods for loading, drawing, updating, and activating scenes.
 * Each scene should inherit from this class and implement the abstract methods.
 */
public abstract class Scene
{
    protected readonly RenderTarget2D target;
    protected readonly SceneManager sceneManager;
    protected Scene(SceneManager sceneManager)
    {
        target = Globals.GetNewRenderTarget();
        this.sceneManager = sceneManager;
        Load();
    }

    /**
     * @brief Loads the scene resources.
     * @details This method should be overridden in derived classes to load specific resources for the scene.
     */
    protected abstract void Load();
    
    /**
     * @brief Draws the scene.
     * @param spriteBatch The SpriteBatch used for drawing.
     * @details This method should be overridden in derived classes to draw the scene's content.
     */
    public abstract void Draw(SpriteBatch spriteBatch);
    
    /**
     * @brief Updates the scene.
     * @param gameTime The current game time.
     * @details This method should be overridden in derived classes to update the scene's state.
     */
    public abstract void Update(GameTime gameTime);
    
    /**
     * @brief Activates the scene.
     * @details This method should be overridden in derived classes to perform any actions needed when the scene is activated.
     */
    public abstract void Activate();
}