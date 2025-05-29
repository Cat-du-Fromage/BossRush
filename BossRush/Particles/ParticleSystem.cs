using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Particles;

public static class ParticleSystem
{
    private static ParticleEmitter emitter { get; } = new();
    public static ParticlePresets presets { get; } = new(emitter);

    public static void Update(GameTime gt) => emitter.Update(gt);
    public static void Draw(SpriteBatch sb) => emitter.Draw(sb);
}