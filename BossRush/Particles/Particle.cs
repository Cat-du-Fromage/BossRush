using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Particles;

public class Particle(Texture2D texture, Vector2 position, Vector2 velocity, Color color, float size, float lifeTime, float rotation = 0f)
{

    private float currentLife = lifeTime;
    private Vector2 _position = position;
    public bool IsActive => currentLife > 0;

    public void Update(GameTime gt)
    {
        if (!IsActive) return;
        float deltaTime = (float)gt.ElapsedGameTime.TotalSeconds;
        _position += velocity * deltaTime;
        currentLife -= deltaTime;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!IsActive) return;
        
        spriteBatch.Draw(
            texture: texture,
            position: _position,
            sourceRectangle: null,
            color: color,
            rotation: rotation,
            origin: new Vector2(texture.Width / 2, texture.Height / 2),
            scale: size,
            effects: SpriteEffects.None,
            layerDepth: 0
        );
    }
}