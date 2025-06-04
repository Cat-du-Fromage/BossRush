using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Animations;

public class Animation(Texture2D spriteSheet, float size, int frameWidth, int frameHeight, float frameTime, int totalFrames = 0)
{
    public Texture2D SpriteSheet { get; } = spriteSheet;
    public int FrameWidth { get; } = frameWidth;
    public int FrameHeight { get; } = frameHeight;
    public int FramesPerRow { get; } = spriteSheet.Width / frameWidth;
    public float FrameTime { get; set; } = frameTime;
    public bool IsLooping { get; set; } = true;
    
    public SpriteEffects Direction { get; set; } = SpriteEffects.None;
    
    private float _timer;
    private int _currentFrame;
    private readonly int _totalFrames = totalFrames > 0
        ? totalFrames
        : (spriteSheet.Width / frameWidth) * (spriteSheet.Height / frameHeight);


    public void Update(GameTime gameTime, SpriteEffects spriteEffects = SpriteEffects.None)
    {
        _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        if (_timer >= FrameTime)
        {
            _timer = 0f;
            _currentFrame++;
            
            if (_currentFrame >= _totalFrames)
            {
                _currentFrame = IsLooping ? 0 : _totalFrames - 1;
            }
        }
        
        Direction = spriteEffects;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color = default)
    {
        if (color == default)
        {
            color = Color.White;
        }

        Rectangle sourceRect = GetCurrentFrameRect();
        spriteBatch.Draw(SpriteSheet, position, sourceRect, color, 0f, Vector2.Zero, size, Direction, 0f);
    }
    
    public Rectangle GetCurrentFrameRect()
    {
        int clampedFrame = Math.Min(_currentFrame, _totalFrames - 1);
        int row = clampedFrame / FramesPerRow;
        int column = clampedFrame % FramesPerRow;
    
        return new Rectangle(
            column * FrameWidth,
            row * FrameHeight,
            FrameWidth,
            FrameHeight
        );
    }

    public void Reset()
    {
        _currentFrame = 0;
        _timer = 0f;
    }
}