using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
// ReSharper disable All

namespace BossRush.Scenes.UIComponents;

public class Button
{
    private const int BUTTON_OFFSET = 2;
    private static readonly Color BACKGROUND_COLOR = new Color(250, 129, 47);
    private static readonly Color COLOR = new Color(255, 178, 44);
    private static readonly Color TEXT_COLOR = new Color(254, 243, 226);
    private static readonly Color HOVER_COLOR = new Color(225,116,42);
    private static readonly Color PRESSED_COLOR = new Color(200,103,37);
    private static readonly Texture2D BUTTON = new Texture2D(Globals.GraphicsDevice, 1, 1);
    private const int WIDTH = 500;
    private const int HEIGHT = 100;
    
    private Rectangle bounds;
    private string text;
    private bool isHovered;
    private bool isPressed;
    private Action onClick;
    public Vector2 position { get; private set; }

    public Button(string text, float x, float y, Action onClick = null)
    {
        this.text = text;
        this.onClick = onClick;
        this.position = new Vector2(x, y);
        this.bounds = new Rectangle((int)x, (int)y, WIDTH, HEIGHT);
    }
    
    
    public void Draw()
    {
        Globals.SpriteBatch.Draw(BUTTON, position, BACKGROUND_COLOR);
        
        Vector2 buttonRect = new Vector2(position.X + BUTTON_OFFSET, position.Y + BUTTON_OFFSET);
        Color currentColor = isPressed ? PRESSED_COLOR :
            isHovered ? HOVER_COLOR :
            COLOR;
        
        Globals.SpriteBatch.Draw(BUTTON, buttonRect, COLOR);
        
        Vector2 textSize = Globals.Font.MeasureString(text);
        Vector2 textPos = new Vector2(
            bounds.X + (bounds.Width - textSize.X) / 2,
            bounds.Y + (bounds.Height - textSize.Y) / 2
        );
    }
    
    public void Update()
    {
        var mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
        isHovered = mouseState.X >= position.X && mouseState.X <= position.X + bounds.Width &&
                    mouseState.Y >= position.Y && mouseState.Y <= position.Y + bounds.Height;
        
        if (isHovered && mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
        {
            isPressed = true;
        }
        else
        {
            isPressed = false;
        }
    }
}