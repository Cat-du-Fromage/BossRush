using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Entities;

public class ProjectileSystem
{
    public static ProjectileSystem Instance { get; private set; }
    public LinkedList<Projectile> Projectiles { get; private set; }

    private ProjectileSystem()
    {
        Projectiles = new LinkedList<Projectile>();
    }

    public static void Initialize()
    {
        Instance = new ProjectileSystem();
    }

    public static void Add(Projectile projectile)
    {
        Instance.Projectiles.AddFirst(projectile);
    }

    public static void Remove(Projectile projectile)
    {
        Instance.Projectiles.Remove(projectile);
    }

    public void Update(GameTime time)
    {
        foreach (var projectile in Projectiles)
        {
            projectile.Update(time);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var projectile in Projectiles)
        {
            projectile.Draw(spriteBatch);
        }
    }
}