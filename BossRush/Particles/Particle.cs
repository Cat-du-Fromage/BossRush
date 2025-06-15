// ================================================================================
// File : Particle.cs
// Project name : BossRush
// Project members :
// - Florian Duruz, Mathieu Rabot, Raphaël Perret
// ================================================================================
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Particles;

/**
 * @brief Represents a particle in the game.
 * @details This class encapsulates the properties and behaviors of a particle, including its texture, position, velocity, color, size, rotation, and lifetime.
 */
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

    /**
     * @brief Updates the particle's position and lifetime based on the elapsed game time.
     * @param gt The current game time.
     */
    public void Update(GameTime gt)
    {
        if (!IsActive) return;
        float deltaTime = (float)gt.ElapsedGameTime.TotalSeconds;
        Position += Velocity * deltaTime;
        CurrentLife -= deltaTime;
    }

    /**
     * @brief Draws the particle using the specified SpriteBatch.
     * @param spriteBatch The SpriteBatch used for drawing the particle.
     */
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