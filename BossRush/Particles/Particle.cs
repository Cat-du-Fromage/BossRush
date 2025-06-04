using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Particles;

public class Particle
{
    public Texture2D Texture;
    public Vector2 Position;
    public Vector2 Velocity;
    public Color Color;
    public float Size;
    public float Rotation;
    public float CurrentLife;
    public float LifeTime;

    public bool IsActive => CurrentLife > 0;

    public void Update(GameTime gt)
    {
        if (!IsActive) return;
        float deltaTime = (float)gt.ElapsedGameTime.TotalSeconds;
        Position += Velocity * deltaTime;
        CurrentLife -= deltaTime;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!IsActive) return;
        
        spriteBatch.Draw(
            texture: Texture,
            position: Position,
            sourceRectangle: null,
            color: Color,
            rotation: Rotation,
            origin: new Vector2(Texture.Width / 2, Texture.Height / 2),
            scale: Size,
            effects: SpriteEffects.None,
            layerDepth: 0
        );
    }
}