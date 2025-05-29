using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Particles;

public class ParticlePresets(ParticleEmitter particleEmitter)
{
    public void CreateMuzzleFlash(Vector2 position, Vector2 direction)
    {
        particleEmitter.CreateParticle(
            texture: Globals.ParticleTextures["muzzle"][0],
            position: position,
            velocity: direction * 0.5f,
            color: Color.Orange,
            size: 1.5f,
            lifeTime: 0.1f
        );
    }
}