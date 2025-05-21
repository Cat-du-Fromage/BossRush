using System;
using System.Globalization;
using BossRush.Scenes.UIComponents;
using Microsoft.Xna.Framework;


namespace BossRush.Scenes;

public class Menu(GameManager gm, SceneManager sm) : Scene(gm, sm)
{

    private Button startButton;
    private Button exitButton;
    
    protected override void Load()
    {
        var startAction = sm.SwitchScene;
        var exitAction = sm.Exit;

        var centerX = Globals.ScreenSize.X / 2;
        var centerY = Globals.ScreenSize.Y / 2;
        
        startButton = new Button("Start", centerX, centerY - 100, startAction);
        exitButton = new Button("Exit", centerX, centerY + 100, exitAction);
    }

    protected override void Draw()
    {
        // Draw a blue background
        Globals.SpriteBatch.Draw(Globals.WhitePixel, new Rectangle(0, 0, (int)Globals.ScreenSize.X, (int)Globals.ScreenSize.Y), new Color(249,239,233));
        startButton.Draw();
        exitButton.Draw();
    }

    public override void Update()
    {
        startButton.Update();
        exitButton.Update();
    }
    
    public override void Activate()
    {
        Load();
    }
}