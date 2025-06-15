// ================================================================================
// File : ParticleEmitter.cs
// Project name : BossRush
// Project members :
// - Florian Duruz, Mathieu Rabot, Raphaël Perret
// ================================================================================
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Particles;

/**
 * @brief Manages the creation and updating of particles in the game.
 * @details This class is responsible for creating particles, updating their state, and drawing them on the screen.
 * It uses a pool of particles to optimize performance by reusing particle objects.
 */
public class ParticleEmitter
{
    private readonly List<Particle> particles = [];
    private readonly Stack<Particle> particlePool = [];

    /**
     * @brief Creates a new particle emitter.
     * @details Initializes the particle emitter with an empty list of particles and a pool for reusing particles.
     * @param texture The texture used for the particles.
     * @param position The initial position of the particles.
     * @param velocity The initial velocity of the particles.
     * @param color The color of the particles.
     * @param size The size of the particles.
     * @param lifeTime The lifetime of the particles in seconds.
     * @param rotation The initial rotation of the particles (default is 0).
     */
    public Particle CreateParticle(Texture2D texture, Vector2 position, Vector2 velocity, Color color, float size,
        float lifeTime, float rotation = 0f)
    {
        Particle particle = particlePool.Count > 0 ? particlePool.Pop() : new Particle();
        particle.Texture = texture;
        particle.Position = position;
        particle.Velocity = velocity;
        particle.Color = color;
        particle.Size = size;
        particle.Rotation = rotation;
        particle.CurrentLife = lifeTime;
        particle.LifeTime = lifeTime; 
        
        particles.Add(particle);
        return particle;
    }
    
    /**
     * @brief Updates the state of all particles in the emitter.
     * @param gt The current game time.
     * @details This method iterates through the list of particles, updates their state, and removes inactive particles.
     */
    public void Update(GameTime gt)
    {
        for (int i = particles.Count - 1; i >= 0; i--)
        {
            Particle particle = particles[i];
            particle.Update(gt);
            if (particle.IsActive) continue;
            particlePool.Push(particle);
            particles.RemoveAt(i);
        }
    }
    
    /**
     * @brief Draws all active particles using the specified SpriteBatch.
     * @param spriteBatch The SpriteBatch used for drawing the particles.
     * @details This method iterates through the list of particles and draws each active particle on the screen.
     */
    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var particle in particles)
        {
            particle.Draw(spriteBatch);
        }
    }
}