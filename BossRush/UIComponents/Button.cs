using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
// ReSharper disable All

namespace BossRush.Scenes.UIComponents;

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
    public Button(string text, int x, int y, Action onClick = null)
    {
        this.text = text;
        this.onClick = onClick;
        outerBounds = new Rectangle(x - (WIDTH / 2), y - (HEIGHT / 2), WIDTH, HEIGHT);
        bounds = new Rectangle(outerBounds.X - BUTTON_OFFSET, outerBounds.Y - BUTTON_OFFSET, outerBounds.Width, outerBounds.Height);
        currentState = ButtonState.Released;
        BUTTON.SetData(new[] { Color.White });
    }
  
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