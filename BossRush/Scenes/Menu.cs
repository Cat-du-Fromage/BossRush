// ================================================================================
// File : Menu.cs
// Project name : BossRush
// Project members :
// - Florian Duruz, Mathieu Rabot, Raphaël Perret
// ================================================================================
using BossRush.Scenes.UIComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace BossRush.Scenes;

/**
 * @brief The Menu scene of the game.
 * @details This scene displays the main menu with options to start the game or exit. It contains buttons for user interaction and handles drawing and updating the menu state.
 * @param sm The SceneManager instance to manage scene transitions and actions.
 */
public class Menu(SceneManager sm) : Scene(sm)
{

    private Button startButton;
    private Button exitButton;
    
    /**
     * @brief Initializes the Menu scene.
     * @details This constructor initializes the Menu scene and calls the Load method to set up the buttons.
     * @param sm The SceneManager instance to manage scene transitions and actions.
     */
    protected override void Load()
    {
        var startAction = sm.SwitchScene;
        var exitAction = sm.Exit;

        var centerX = Globals.ScreenSize().X / 2;
        var centerY = Globals.ScreenSize().Y / 2;
        
        startButton = new Button("Start", centerX, centerY - 100, startAction);
        exitButton = new Button("Exit", centerX, centerY + 100, exitAction);
    }

    /**
     * @brief Draws the Menu scene.
     * @param spriteBatch The SpriteBatch used for drawing the scene.
     * @details This method draws the background and the buttons on the screen.
     */
    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(Globals.WhitePixel,
            new Rectangle(0, 0, Globals.ScreenSize().X, Globals.ScreenSize().Y),
            new Color(249,239,233));
        startButton.Draw(spriteBatch);
        exitButton.Draw(spriteBatch);
        spriteBatch.End();
    }

    /**
     * @brief Updates the Menu scene.
     * @param gameTime The current game time.
     * @details This method updates the state of the buttons in the menu.
     */
    public override void Update(GameTime gameTime)
    {
        startButton.Update();
        exitButton.Update();
    }
    
    /**
     * @brief Activates the Menu scene.
     * @details This method is called when the scene is activated, and it loads the necessary resources for the menu.
     */
    public override void Activate()
    {
        Load();
    }
}