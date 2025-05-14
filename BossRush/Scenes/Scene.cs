using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

namespace BossRush.Scenes;

public abstract class Scene
{
    protected readonly RenderTarget2D target;
    protected readonly GameManager game;
    protected readonly SceneManager sceneManager;

    protected Scene(GameManager game, SceneManager sceneManager)
    {
        this.game = game;
        target = Globals.GetNewRenderTarget();
        this.sceneManager = sceneManager;
        Load();
    }

    protected abstract void Load();
    protected abstract void Draw();
    public abstract void Update();
    public abstract void Activate();

    public virtual RenderTarget2D GetFrame()
    {
        Globals.GraphicsDevice.SetRenderTarget(target);
        Globals.GraphicsDevice.Clear(Color.Transparent);

        Globals.SpriteBatch.Begin();
        Draw();
        Globals.SpriteBatch.End();

        Globals.GraphicsDevice.SetRenderTarget(null);
        return target;
    }
}