using System;
using System.Collections.Generic;
using BossRush.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game = BossRush.Scenes.Game;

namespace BossRush;

public class SceneManager
{
    private Scenes.Scenes CurrentScene { get; set; }
    private readonly Dictionary<Scenes.Scenes, Scene> scenes = [];

    public SceneManager(GameManager gm)
    {
        scenes.Add(Scenes.Scenes.Menu, new Menu(gm, this));
        scenes.Add(Scenes.Scenes.Game, new Game(gm, this));
        
        CurrentScene = Scenes.Scenes.Menu;
        scenes[CurrentScene].Activate();
    }
    
    public void Update(GameTime gameTime)
    {
        scenes[CurrentScene].Update(gameTime);
    }
    
    public void SwitchScene()
    {
        CurrentScene = CurrentScene == Scenes.Scenes.Menu ? Scenes.Scenes.Game : Scenes.Scenes.Menu;
        scenes[CurrentScene].Activate();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        scenes[CurrentScene].Draw(spriteBatch);
    }
    
    public void Exit()
    {
        Globals.GameInstance.Exit();
    }
}