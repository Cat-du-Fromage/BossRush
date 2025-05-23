using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

namespace BossRush.Scenes;

public abstract class Scene
{
    protected readonly RenderTarget2D target;
    protected readonly SceneManager sceneManager;
    protected Scene(SceneManager sceneManager)
    {
        target = Globals.GetNewRenderTarget();
        this.sceneManager = sceneManager;
        Load();
    }

    protected abstract void Load();
    public abstract void Draw(SpriteBatch spriteBatch);
    public abstract void Update(GameTime gameTime);
    public abstract void Activate();
}