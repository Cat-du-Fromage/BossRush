using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Scenes;

public class Game(GameManager gm, SceneManager sm) : Scene(gm, sm)
{
    
    protected override void Load()
    {
        //throw new System.NotImplementedException();
    }

    protected override void Draw()
    {
        Globals.SpriteBatch.Draw(Globals.WhitePixel, new Rectangle(0, 0, (int)Globals.ScreenSize.X, (int)Globals.ScreenSize.Y), new Color(160, 200, 120));
        //throw new System.NotImplementedException();
    }

    public override void Update()
    {
        //throw new System.NotImplementedException();
    }

    public override void Activate()
    {
        Load();
    }
}