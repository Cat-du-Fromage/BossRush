// ================================================================================
// File : Button.cs
// Project name : BossRush
// Project members :
// - Florian Duruz, Mathieu Rabot, Raphaël Perret
// ================================================================================
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace BossRush.Scenes.UIComponents;

/**
 * @brief Represents a button in the UI.
 * @details This class handles the button's appearance, state, and interaction. It can be drawn on the screen and responds to mouse input.
 */
public class Button
{
    private const int BUTTON_OFFSET = 5;
    private static readonly Color BACKGROUND_COLOR = new Color(250, 129, 47);
    private static readonly Color COLOR = new Color(255, 178, 44);
    private static readonly Color TEXT_COLOR = new Color(254, 243, 226);
    private static readonly Color HOVER_COLOR = new Color(225,116,42);
    private static readonly Color PRESSED_COLOR = new Color(200,103,37);
    private static readonly Texture2D BUTTON = new Texture2D(Globals.GraphicsDevice, 1, 1);
    private const int WIDTH = 500;
    private const int HEIGHT = 100;
    private ButtonState currentState;
    
    private Rectangle bounds;
    private Rectangle outerBounds;
    private string text;
    private bool isHovered;
    private bool isPressed;
    private Action onClick;
    public Vector2 position { get; private set; }
    
    /**
     * @brief Initializes a new instance of the Button class.
     * @param text The text displayed on the button.
     * @param x The x-coordinate of the button's center.
     * @param y The y-coordinate of the button's center.
     * @param onClick An optional action to perform when the button is clicked.
     */
    public Button(string text, int x, int y, Action onClick = null)
    {
        this.text = text;
        this.onClick = onClick;
        outerBounds = new Rectangle(x - (WIDTH / 2), y - (HEIGHT / 2), WIDTH, HEIGHT);
        bounds = new Rectangle(outerBounds.X - BUTTON_OFFSET, outerBounds.Y - BUTTON_OFFSET, outerBounds.Width, outerBounds.Height);
        currentState = ButtonState.Released;
        BUTTON.SetData(new[] { Color.White });
    }
  
    /**
     * @brief Draws the button on the screen.
     * @param spriteBatch The SpriteBatch used for drawing the button.
     */
    public void Draw(SpriteBatch spriteBatch)
    {
        // Draw the button background
        spriteBatch.Draw(BUTTON, bounds, BACKGROUND_COLOR);

        // Draw the button foreground with the current state color
        Color currentColor = isPressed ? PRESSED_COLOR :
            isHovered ? HOVER_COLOR :
            COLOR;
        spriteBatch.Draw(BUTTON, outerBounds, currentColor);

        // Draw the button text
        Vector2 textSize = Globals.Font.MeasureString(text);
        Vector2 textPos = new Vector2(
            outerBounds.X + (outerBounds.Width - textSize.X) / 2,
            outerBounds.Y + (outerBounds.Height - textSize.Y) / 2
        );
        spriteBatch.DrawString(Globals.Font, text, textPos, TEXT_COLOR);

    }
    
    /**
     * @brief Updates the button state based on mouse input.
     * @details This method checks if the mouse is hovering over the button and if it is pressed, triggering the onClick action if applicable.
     */
    public void Update()
    {
        var mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
        isHovered = mouseState.X >= outerBounds.X && mouseState.X <= outerBounds.X + outerBounds.Width &&
                    mouseState.Y >= outerBounds.Y && mouseState.Y <= outerBounds.Y + outerBounds.Height;
    
        bool wasPressed = isPressed;
        isPressed = isHovered && mouseState.LeftButton == ButtonState.Pressed;
    
        if (!wasPressed && isPressed && onClick != null)
        {
            onClick.Invoke();
        }
    }
}