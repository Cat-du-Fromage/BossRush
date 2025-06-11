using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Particles;

public class ParticlePresets(ParticleEmitter particleEmitter)
{
    private const float MAX_SIZE = 0.08f;
    public void CreateMuzzleFlash(Vector2 position, Vector2 direction)
    {
        particleEmitter.CreateParticle(
            texture: Globals.ParticleTextures["muzzle"][0],
            position: position,
            velocity: direction * 0.5f,
            color: Color.Orange,
            size: MAX_SIZE,
            lifeTime: 0.1f
        );
    }

    public void CreateFlash(Vector2 position, Vector2 direction)
    {
        particleEmitter.CreateParticle(
            texture: Globals.ParticleTextures["fire"][1],
            position: position,
            velocity: Vector2.Zero,
            color: Color.DarkGray,
            size: MAX_SIZE / 4,
            lifeTime: 0.5f
        );
    }
    
    public void CreateSplash(Vector2 position, Vector2 direction)
    {
        particleEmitter.CreateParticle(
            texture: Globals.ParticleTextures["dirt"][0],
            position: position,
            velocity: Vector2.Zero,
            color: Color.Red,
            size: MAX_SIZE / 2,
            lifeTime: 0.5f
        );
    }
    
    public void CreateSlashEffect(Vector2 position, Vector2 direction)
    {
        particleEmitter.CreateParticle(
            texture: Globals.ParticleTextures["slash"][0],
            position: position,
            velocity: direction * 0.5f,
            color: Color.Orange,
            size: MAX_SIZE,
            lifeTime: 0.1f
        );
    }
}