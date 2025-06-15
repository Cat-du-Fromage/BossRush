// ================================================================================
// File : ParticleSystem.cs
// Project name : BossRush
// Project members :
// - Florian Duruz, Mathieu Rabot, Raphaël Perret
// ================================================================================
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Particles;

/**
 * @brief Manages the particle system in the game.
 * @details This class is responsible for initializing, updating, and drawing particles using an emitter.
 * It provides access to predefined particle presets through the ParticlePresets class.
 */
public class ParticleSystem
{
    private readonly ParticleEmitter emitter;
    public ParticlePresets Presets { get; }
    public static ParticleSystem Instance { get; private set; }
    
    /**
     * @brief Private constructor to prevent instantiation from outside.
     * @details Initializes the ParticleEmitter and creates an instance of ParticlePresets.
     */
    private ParticleSystem()
    {
        emitter = new ParticleEmitter();
        Presets = new ParticlePresets(emitter);
    }
    
    /**
     * @brief Initializes the ParticleSystem singleton instance.
     * @details This method checks if the Instance is null and creates a new ParticleSystem if it is.
     * It should be called once at the start of the game to ensure the particle system is ready to use.
     */
    public static void Initialize()
    {
        Instance ??= new ParticleSystem();
    }
    
    /**
     * @brief Gets the ParticleEmitter used by the ParticleSystem.
     * @return The ParticleEmitter instance.
     */
    public void Update(GameTime gt) => emitter.Update(gt);
    
    /**
     * @brief Draws the particles using the ParticleEmitter.
     * @param sb The SpriteBatch used for drawing the particles.
     * @details This method calls the Draw method of the ParticleEmitter to render all active particles on the screen.
     */
    public void Draw(SpriteBatch sb) => emitter.Draw(sb);
}