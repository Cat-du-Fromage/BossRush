using System;
using Microsoft.Xna.Framework;

namespace BossRush.Entities;

public interface IAbility
{
    void Use(EntityBase caster, Point target);
}

static class Utility
{
    public static EntityBase FindClosest(EntityBase caster, Point target, float range)
    {
        //find closest target entity
        EntityBase closest = null;
        float closestSquaredDistance = float.MaxValue;
        foreach (var gameComponent in Game1.Instance.Components)
        {
            if (gameComponent is EntityBase @entity)
            {
                if(entity == caster)
                    continue;
                Vector2 ct = @entity.Position - target.ToVector2();
                if (ct.LengthSquared() < closestSquaredDistance)
                {
                    closestSquaredDistance = ct.LengthSquared();
                    closest = @entity;
                }
            }
        }
        
        // Ensure it is within reasonable distance
        if (closestSquaredDistance > range * range)
            return null;
        
        return closest;
    }

    public static Vector2 SetLength(Vector2 vector, float length)
    {
        if (vector.LengthSquared() == 0)
            return vector;
        vector.Normalize(); 
        return vector * length;
    }

    public static Vector2 Projection(Vector2 v, Vector2 n)
    {
        return Vector2.Dot(v, n) / n.LengthSquared() * n;
    }
}

public class Arrow : IAbility
{
    private static readonly Projectile.Builder Builder = new Projectile.Builder();
    static bool _configured;
    public void Use(EntityBase caster, Point target)
    {
        if (!_configured)
        {
            _configured = true;
            Builder.SetDirect(null)
                .SetFriction(0.5f)
                .SetKnockback(1)
                .SetDamage(10)
                .SetMaxAcceleration(0)
                .SetLifeSpan(TimeSpan.FromSeconds(20))
                .SetMaxSpeed(500)
                .SetSize(8)
                .SetGame(Game1.Instance);
        }
        Vector2 pt = target.ToVector2() - caster.Position;
        pt = Utility.SetLength(pt, Builder.MaxSpeed);
        Game1.Instance.Components.Add(
            Builder.SetOwner(caster)
            .SetVelocity(pt)
            .SetPosition(caster.Position)
            .Build()
        );
    }
}

public class HomingMagic : IAbility
{
    private static readonly Projectile.Builder Builder = new Projectile.Builder();
    static bool _configured;
    public void Use(EntityBase caster, Point target)
    {
        if (!_configured)
        {
            _configured = true;
            Projectile.Director.MakeGuided(Builder);
            Builder.SetDamage(10)
                .SetFriction(0.0f)
                .SetMaxAcceleration(150)
                .SetGame(Game1.Instance)
                .SetMaxSpeed(75);
        }
        EntityBase closest = Utility.FindClosest(caster, target, 100);
        if (closest == null)
            return;
        
        Game1.Instance.Components.Add(
            Builder.SetOwner(caster)
                .SetTarget(closest)
                .SetPosition(caster.Position)
                .SetVelocity(Vector2.Zero)
                .Build()
        );

    }
}

public class AchillesArrow : IAbility
{
    private static readonly Projectile.Builder Builder = new Projectile.Builder();
    private static bool _configured;
    public void Use(EntityBase caster, Point target)
    {
        if (!_configured)
        {
            _configured = true;
            Builder.SetFriction(0.4f)
                .SetKnockback(1)
                .SetDamage(10)
                .SetMaxAcceleration(500)
                .SetMaxSpeed(600)
                .SetLifeSpan(TimeSpan.FromSeconds(20))
                .SetSize(10)
                .SetGame(Game1.Instance)
                .SetDirect(projectile =>
                {
                    Vector2 v = projectile.GetVelocity();
                    float scalar = v.Length();
                    Vector2 at = projectile.TargetEntity.Position - projectile.Position;
                    Vector2 orthogonal = new Vector2(v.Y, -v.X);
                    at.Normalize();
                    return Utility.Projection(at, orthogonal) * scalar;
                });
        }
        
        EntityBase closest = Utility.FindClosest(caster, target, 400);
        if (closest == null)
            return;
        
        Vector2 pt = target.ToVector2() - caster.Position;
        pt = Utility.SetLength(pt, Builder.MaxSpeed);
        
        Game1.Instance.Components.Add(
            Builder.SetOwner(caster)
                .SetTarget(closest)
                .SetPosition(caster.Position)
                .SetVelocity(pt)
                .Build()
        );
        
    }
}

public class Defense : IAbility
{
    private static readonly Projectile.Builder Builder = new Projectile.Builder();
    private static bool _configured;

    public void Use(EntityBase caster, Point target)
    {
        if (!_configured)
        {
            _configured = true;
            Builder.SetDamage(10)
                .SetFriction(0)
                .SetMaxAcceleration(500)
                .SetMaxSpeed(400)
                .SetGame(Game1.Instance)
                .SetSize(5)
                .SetVelocity(Vector2.Zero);
            Projectile.Director.MakeGuided(Builder);
        }
            
        EntityBase closest = Utility.FindClosest(caster, caster.Position.ToPoint(), 100);
        if (closest == null)
            return;

        Builder.Game.Components.Add(
            Builder.SetOwner(caster)
                .SetTarget(closest)
                .SetPosition(caster.Position)
                .Build()
        );
    }
}

public class ExplosiveMagic : IAbility
{
    private static readonly Projectile.Builder Builder = new Projectile.Builder();
    private static bool _configured;
    public void Use(EntityBase caster, Point target)
    {
        if (!_configured)
        {
            _configured = true;
            Projectile.Director.MakeGuided(Builder);
            Builder.SetDamage(10)
                .SetFriction(0.0f)
                .SetMaxAcceleration(500)
                .SetGame(Game1.Instance)
                .SetMaxSpeed(300)
                .SetDeath(projectile =>
                {
                    // The explosion is simply a huge static projectile that isn't destroyed on impact and has a short life
                    Game1.Instance.Components.Add(
                        new Projectile.Builder()
                            .SetOwner(null)  // explosion will hurt the casters
                            .SetGame(Game1.Instance)
                            .SetLifeSpan(TimeSpan.FromSeconds(1))
                            .setDiesOnImpact(false)
                            .SetMaxSpeed(0)
                            .SetVelocity(Vector2.Zero)
                            .SetPosition(projectile.Position)
                            .SetSize(50)
                            .Build()
                        );
                });
        }
        EntityBase closest = Utility.FindClosest(caster, target, 100);
        if (closest == null)
            return;
        
        Game1.Instance.Components.Add(
            Builder.SetOwner(caster)
                .SetTarget(closest)
                .SetPosition(caster.Position)
                .SetVelocity(Vector2.Zero)
                .Build()
        );

    }
}