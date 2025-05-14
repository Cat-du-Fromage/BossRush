using System;
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
        
        float centerX = Globals.ScreenSize.X / 2;
        float centerY = Globals.ScreenSize.Y / 2;
        Console.WriteLine("Loading Menu");

        startButton = new Button("Start", centerX - 100, centerY - 100, startAction);
        exitButton = new Button("Exit", centerX - 100, centerY + 100, exitAction);
    }

    protected override void Draw()
    {
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