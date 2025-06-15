// ================================================================================
// File : ParticlePresets.cs
// Project name : BossRush
// Project members :
// - Florian Duruz, Mathieu Rabot, Raphaël Perret
// ================================================================================
using Microsoft.Xna.Framework;

namespace BossRush.Particles;

/**
    * @brief Contains preset particle effects for the game.
    * @details This class provides methods to create various particle effects such as muzzle flashes, fire flashes, splashes, and slash effects.
    * Each method uses a ParticleEmitter to create particles with specific textures, positions, velocities, colors, sizes, and lifetimes.
 */
public class ParticlePresets(ParticleEmitter particleEmitter)
{
    private const float MAX_SIZE = 0.08f;
    /**
     * @brief Creates a muzzle flash particle effect.
     * @param position The position where the muzzle flash should be created.
     * @param direction The direction of the muzzle flash.
     * @details This method creates a particle with a specific texture, position, velocity, color, size, and lifetime.
     * @used by the weapon system to simulate the visual effect of a gun firing.
     */
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

    /**
     * @brief Creates a fire flash particle effect.
     * @param position The position where the fire flash should be created.
     * @param direction The direction of the fire flash.
     * @details This method creates a particle with a specific texture, position, velocity, color, size, and lifetime.
     * @used by the weapon system to simulate the visual effect of fire or explosions.
     */
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
    
    /**
     * @brief Creates a splash particle effect.
     * @param position The position where the splash should be created.
     * @param direction The direction of the splash.
     * @details This method creates a particle with a specific texture, position, velocity, color, size, and lifetime.
     * @used by the weapon system to simulate the visual effect of impacts on hits or surfaces.
     */
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
    
    /**
     * @brief Creates a slash particle effect.
     * @param position The position where the slash should be created.
     * @param direction The direction of the slash.
     * @details This method creates a particle with a specific texture, position, velocity, color, size, and lifetime.
     * @used by the weapon system to simulate the visual effect of slashing attacks.
     */
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