using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Particles;

public class ParticleEmitter
{
    private readonly List<Particle> particles = [];
    private readonly Stack<Particle> particlePool = [];

    public Particle CreateParticle(Texture2D texture, Vector2 position, Vector2 velocity, Color color, float size,
        float lifeTime, float rotation = 0f)
    {
        Particle particle = particlePool.Count > 0 ? particlePool.Pop() : new Particle(
            texture, position, velocity, color, size, lifeTime, rotation);
        
        particles.Add(particle);
        return particle;
    }
    
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
    
    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var particle in particles)
        {
            particle.Draw(spriteBatch);
        }
    }
}