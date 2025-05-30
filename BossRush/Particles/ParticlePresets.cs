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
}