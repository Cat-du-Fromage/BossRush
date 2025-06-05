using System;
using Microsoft.Xna.Framework;

namespace BossRush.Entities;

public interface IProjectileDirector // This is a strategy implementation of the director
{
    Projectile.Builder Apply(Projectile.Builder builder);
}

public class Homing : IProjectileDirector
{
    public Projectile.Builder Apply(Projectile.Builder builder)
    {
        return builder.SetDirect(projectile =>
        {
            // If target is spinning, this projectile might follow it and circle endlessly
            // To stop this from happening we are going to cancel currentVelocity by a factor representing how close it is to being orthogonal to PT
            Vector2 pt = projectile.TargetEntity.Position - projectile.Position;
            pt.Normalize();
                
            Vector2 currentVelocity = projectile.GetVelocity();
            if (currentVelocity.LengthSquared() != 0)
            {
                currentVelocity.Normalize();
                pt -= currentVelocity * (1 - Math.Abs(Vector2.Dot(pt, currentVelocity)));
            }

            pt *= projectile.MaxAcceleration;
            return pt;
        });
    }
}

public class Arrow : IProjectileDirector
{ public Projectile.Builder Apply(Projectile.Builder builder)
    {
        return builder
            .SetDirect(null)
            .SetSize(8)
            .SetFriction(0.5f)
            .SetMaxAcceleration(0)
            .SetLifeSpan(TimeSpan.FromSeconds(10))
            .SetMaxSpeed(500);
    }
}

public class ConservativeHoming : IProjectileDirector
{
    public Projectile.Builder Apply(Projectile.Builder builder)
    {
        return builder
            .SetFriction(0.4f)
            .SetMaxAcceleration(500)
            .SetMaxSpeed(600)
            .SetLifeSpan(TimeSpan.FromSeconds(20))
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
}

public class Explosive : IProjectileDirector
{ 
    public Projectile.Builder Apply(Projectile.Builder builder)
    {
        return builder.SetDeath(projectile =>
        {
            // The explosion is simply a huge static projectile that isn't destroyed on impact and has a short life
            ProjectileSystem.Add(
                new Projectile.Builder()
                    .SetOwner(null)  // explosion will hurt the casters
                    .SetLifeSpan(TimeSpan.FromSeconds(0.3))
                    .SetImpactResistance(int.MaxValue)
                    .SetMaxSpeed(0)
                    .SetVelocity(Vector2.Zero)
                    .SetPosition(projectile.Position)
                    .SetSize(50)
                    .SetTexture(Globals.ParticleTextures["scorch"][2])
                    .SetColor(Color.DarkOrange)
                    .Build()
            );
        });
    }
}

// Temporary zone that does not collide but spawns one that does on death
public class DangerZone(TimeSpan delay) : IProjectileDirector
{
    public Projectile.Builder Apply(Projectile.Builder builder)
    {
        Projectile.Builder copy = builder.Clone();
        return builder
            .SetMaxSpeed(0)
            .SetDeath(projectile =>
            {
                ProjectileSystem.Add(
                    copy.SetPosition(projectile.Position)
                        .SetOwner(projectile.Owner)
                        .Build());
            })
            .SetLifeSpan(delay)
            .SetImpactResistance(int.MaxValue)
            .SetTexture(Globals.ParticleTextures["circle"][0])
            .SetColor(Color.DarkRed);
    }
}

public class MoveToCaster : IProjectileDirector
{
    public Projectile.Builder Apply(Projectile.Builder builder)
    {
        return builder.SetDirect(projectile =>
        {
            Vector2 pc = projectile.Owner.Position - projectile.Position;
            return pc;
        });
    }
}


public abstract class ParticleEffect : IProjectileDirector
{
    protected ParticleEffect(Action<Vector2, Vector2> creatParticle)
    {
        CreateParticle = creatParticle;
    }
    protected Action<Vector2, Vector2> CreateParticle;
    
    public abstract Projectile.Builder Apply(Projectile.Builder builder);
}
public class TrailEffect : ParticleEffect
{
    public TrailEffect(Action<Vector2, Vector2> creatParticle) : base(creatParticle) {}
    public override Projectile.Builder Apply(Projectile.Builder builder)
    {
        return builder.SetOnUpdate((projectile =>
        {
            CreateParticle.Invoke(projectile.Position, projectile.GetVelocity());
        }));
    }
}

public class FireEffect : ParticleEffect
{
    public FireEffect(Action<Vector2, Vector2> creatParticle) : base(creatParticle) {}
    public override Projectile.Builder Apply(Projectile.Builder builder)
    {
        return builder.SetOnFire(projectile =>
        {
            CreateParticle.Invoke(projectile.Position, Vector2.Zero);
        });
    }
}

public class HitEffect : ParticleEffect
{ 
    public HitEffect(Action<Vector2, Vector2> creatParticle) : base(creatParticle) {}
    public override Projectile.Builder Apply(Projectile.Builder builder)
    {
        return builder.SetOnHit((projectile =>
        {
            CreateParticle.Invoke(projectile.Position, projectile.GetVelocity());
        }));
    }
}