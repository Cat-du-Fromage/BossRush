// ================================================================================
// File : SceneManager.cs
// Project name : BossRush
// Project members :
// - Florian Duruz, Mathieu Rabot, Raphaël Perret
// ================================================================================

using System.Collections.Generic;
using BossRush.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game = BossRush.Scenes.Game;

namespace BossRush;

/**
 * @brief Manages the different scenes in the game.
 * @details This class is responsible for switching between scenes, updating the current scene, and drawing the current scene.
 * It initializes the scenes and handles scene transitions.
 */
public class SceneManager
{
    private Scenes.Scenes currentScene { get; set; }
    private readonly Dictionary<Scenes.Scenes, Scene> SCENES = [];

    /**
     * @brief Initializes the SceneManager and sets up the scenes.
     * @details This constructor initializes the Menu and Game scenes and sets the current scene to Menu.
     */
    public SceneManager()
    {
        SCENES.Add(Scenes.Scenes.Menu, new Menu(this));
        SCENES.Add(Scenes.Scenes.Game, new Game(this));
        
        currentScene = Scenes.Scenes.Menu;
        SCENES[currentScene].Activate();
    }
    
    /**
     * @brief Updates the current scene.
     * @param gameTime The current game time.
     * @details This method calls the Update method of the current scene to update its state.
     */
    public void Update(GameTime gameTime)
    {
        SCENES[currentScene].Update(gameTime);
    }
    
    /**
     * @brief Switches the current scene.
     * @details This method toggles between the Menu and Game scenes. If the current scene is Menu, it switches to Game, and vice versa.
     */
    public void SwitchScene()
    {
        currentScene = currentScene == Scenes.Scenes.Menu ? Scenes.Scenes.Game : Scenes.Scenes.Menu;
        SCENES[currentScene].Activate();
    }

    /**
     * @brief Draws the current scene.
     * @param spriteBatch The SpriteBatch used for drawing the scene.
     * @details This method calls the Draw method of the current scene to render its content.
     */
    public void Draw(SpriteBatch spriteBatch)
    {
        SCENES[currentScene].Draw(spriteBatch);
    }
    
    /**
     * @brief Exits the game.
     * @details This method calls the Exit method of the Game instance to terminate the application.
     */
    public void Exit()
    {
        Globals.GameInstance.Exit();
    }
}