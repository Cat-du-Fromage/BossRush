using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Enemy;

public class EnemySystem
{
    private static EnemySystem instance;
    public static EnemySystem Instance => instance ??= new EnemySystem();
//╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
//║                                               ◆◆◆◆◆◆ PROPERTIES ◆◆◆◆◆◆                                             ║
//╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝
    public List<Enemy> Enemies { get; private set; } = new();
    
//╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
//║                                             ◆◆◆◆◆◆ CONSTRUCTOR ◆◆◆◆◆◆                                              ║
//╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝

    public EnemySystem()
    {
        instance = this;
    }
    
//╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
//║                                           ◆◆◆◆◆◆ MONOGAME EVENTS ◆◆◆◆◆◆                                            ║
//╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝
    public void Update(GameTime gameTime, Vector2 target)
    {
        for (int i = Enemies.Count - 1; i > -1; i--)
        {
            if (Enemies[i].CurrentHealth <= 0)
            {
                Enemies.RemoveAt(i);
            }
            else
            {
                Enemies[i].OnUpdate(gameTime, target);
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        foreach (Enemy enemy in Enemies)
        {
            enemy.Draw(spriteBatch);
        }
        spriteBatch.End();
    }
    
//╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
//║                                            ◆◆◆◆◆◆ CLASS METHODS ◆◆◆◆◆◆                                             ║
//╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝

    public void Register(Enemy enemy)
    {
        if (Enemies.Contains(enemy)) return;
        Enemies.Add(enemy);
    }

    public void Unregister(Enemy enemy)
    {
        Enemies.Remove(enemy);
    }
}