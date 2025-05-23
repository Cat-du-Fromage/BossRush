using System;
using System.Globalization;
using BossRush.Scenes.UIComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace BossRush.Scenes;

public class Menu(SceneManager sm) : Scene(sm)
{

    private Button startButton;
    private Button exitButton;
    
    protected override void Load()
    {
        var startAction = sm.SwitchScene;
        var exitAction = sm.Exit;

        var centerX = Globals.ScreenSize().X / 2;
        var centerY = Globals.ScreenSize().Y / 2;
        
        startButton = new Button("Start", centerX, centerY - 100, startAction);
        exitButton = new Button("Exit", centerX, centerY + 100, exitAction);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(Globals.WhitePixel,
            new Rectangle(0, 0, Globals.ScreenSize().X, Globals.ScreenSize().Y),
            new Color(249,239,233));
        startButton.Draw(spriteBatch);
        exitButton.Draw(spriteBatch);
        spriteBatch.End();
    }

    public override void Update(GameTime gameTime)
    {
        startButton.Update();
        exitButton.Update();
    }
    
    public override void Activate()
    {
        Load();
    }
}