// ================================================================================
// File : Animation.cs
// Project name : BossRush
// Project members :
// - Florian Duruz, Mathieu Rabot, Raphaël Perret
// ================================================================================
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Animations;

/**
 * @brief A class to handle sprite animations using a sprite sheet.
 * @details This class allows you to create animations from a sprite sheet by specifying the frame dimensions, frame time, and total frames. It supports looping animations and can draw the current frame at a specified position with a given color.
 * @param spriteSheet The texture containing the animation frames.
 * @param size The scale factor for the animation.
 * @param frameWidth The width of each frame in the sprite sheet.
 * @param frameHeight The height of each frame in the sprite sheet.
 * @param frameTime The time each frame is displayed before switching to the next.
 * @param totalFrames The total number of frames in the animation. If 0, it will be calculated based on the sprite sheet dimensions.
 */
public class Animation(Texture2D spriteSheet, float size, int frameWidth, int frameHeight, float frameTime, int totalFrames = 0)
{
    private Texture2D spriteSheet { get; } = spriteSheet;
    private int frameWidth { get; } = frameWidth;
    private int frameHeight { get; } = frameHeight;
    private int framesPerRow { get; } = spriteSheet.Width / frameWidth;
    private float frameTime { get; } = frameTime;
    private bool isLooping => true;

    private SpriteEffects direction { get; set; } = SpriteEffects.None;
    
    private float timer;
    private int currentFrame;
    private readonly int totalFrames = totalFrames > 0
        ? totalFrames
        : (spriteSheet.Width / frameWidth) * (spriteSheet.Height / frameHeight);


    /**
     * @brief Updates the animation state based on the elapsed game time.
     * @param gameTime The current game time.
     * @param spriteEffects The sprite effects to apply, can be SpriteEffects.None or SpriteEffects.FlipHorizontally
     */
    public void Update(GameTime gameTime, SpriteEffects spriteEffects = SpriteEffects.None)
    {
        timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        if (timer >= frameTime)
        {
            timer = 0f;
            currentFrame++;
            
            if (currentFrame >= totalFrames)
            {
                currentFrame = isLooping ? 0 : totalFrames - 1;
            }
        }
        
        direction = spriteEffects;
    }

    /**
     * @brief Draws the current frame of the animation at the specified position.
     * @param spriteBatch The SpriteBatch used for drawing.
     * @param position The position to draw the animation frame.
     * @param color The color to apply to the frame, defaults to Color.White.
     */
    public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color = default)
    {
        if (color == default)
        {
            color = Color.White;
        }

        Rectangle sourceRect = GetCurrentFrameRect();
        spriteBatch.Draw(spriteSheet, position, sourceRect, color, 0f, Vector2.Zero, size, direction, 0f);
    }

    /**
     * @brief Gets the rectangle representing the current frame in the sprite sheet.
     * @details On a 3x3 sprite sheet with frame size 32x32, the first frame would be at (0,0), second at (32,0), etc.
     * @return A Rectangle representing the current frame's position and size in the sprite sheet.
     */
    private Rectangle GetCurrentFrameRect()
    {
        int clampedFrame = Math.Min(currentFrame, totalFrames - 1);
        int row = clampedFrame / framesPerRow;
        int column = clampedFrame % framesPerRow;
    
        return new Rectangle(
            column * frameWidth,
            row * frameHeight,
            frameWidth,
            frameHeight
        );
    }

    /**
     * @brief Resets the animation to the first frame.
     */
    public void Reset()
    {
        currentFrame = 0;
        timer = 0f;
    }
}