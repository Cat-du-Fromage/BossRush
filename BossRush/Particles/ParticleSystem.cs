using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Particles;

public class ParticleSystem
{
    private readonly ParticleEmitter emitter;
    public ParticlePresets Presets { get; }
    public static ParticleSystem Instance { get; private set; }
    
    private ParticleSystem()
    {
        emitter = new ParticleEmitter();
        Presets = new ParticlePresets(emitter);
    }
    
    public static void Initialize()
    {
        Instance ??= new ParticleSystem();
    }
    
    public void Update(GameTime gt) => emitter.Update(gt);
    public void Draw(SpriteBatch sb) => emitter.Draw(sb);
}