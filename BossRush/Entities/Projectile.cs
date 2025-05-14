using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Entities;

public class Projectile : EntityBase
{
    private Projectile(Game game, Vector2 pos, Vector2 vel) : base(pos,vel,game){}

    public override bool IsAlive()
    {
        throw new NotImplementedException();
    }
    
    public interface IDirect
    {
        public abstract Vector2 GetAcceleration(Projectile projectile);
    }

    public interface IDeath
    {
        public abstract void OnDeath();
    }

    public float MaxSpeed { get; private set; }
    public float MaxAcceleration{ get; private set; }
    public Vector2 TargetPosition{ get; private set; }
    
    public EntityBase Owner{ get; private set; }
    public EntityBase TargetEntity{ get; private set; }

    public float Damage{ get; private set; }
    public float Knockback{ get; private set; }
    public float Stun{ get; private set; }
    
    public IDirect Direct{ get; private set; }
    public IDeath Death{ get; private set; }

    public class Builder
    {
        public Game Game { get; private set; }
        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; private set; }
        public float MaxSpeed { get; private set; }
        public float MaxAcceleration{ get; private set; }
        public Vector2 TargetPosition{ get; private set; }
    
        public EntityBase Owner{ get; private set; }
        public EntityBase TargetEntity{ get; private set; }

        public float Damage{ get; private set; }
        public float Knockback{ get; private set; }
        public float Stun{ get; private set; }
        public IDirect Direct{ get; private set; }
        public IDeath Death{ get; private set; }
        
        Builder setDirect(IDirect direct)
        {
                Direct =  direct;
                return this;
        }

        Builder setDeath(IDeath death)
        {
            Death = death;
            return this;
        }

        Builder setMaxSpeed(float maxSpeed)
        {
            MaxSpeed = maxSpeed;
            return this;
        }

        Builder setMaxAcceleration(float maxAcceleration)
        {
            MaxAcceleration = maxAcceleration;
            return this;
        }

        Builder setStun(float stun)
        {
            Stun = stun;
            return this;
        }

        Builder setDamage(float damage)
        {
            Damage = damage;
            return this;
        }

        Builder setKnockback(float knockback)
        {
            Knockback =  knockback;
            return this;
        }

        Builder setOwner(EntityBase owner)
        {
            Owner = owner;
            return this;
        }

        Builder setTarget(EntityBase target)
        {
            TargetEntity =  target;
            return this;
        }

        Builder setTarget(Vector2 target)
        {
            TargetPosition =  target;
            return this;
        }
    }

    private Projectile(Builder builder) : base(builder.Position,builder.Velocity,builder.Game)
    {
        MaxSpeed = builder.MaxSpeed;
        MaxAcceleration = builder.MaxAcceleration;
        TargetPosition = builder.TargetPosition;
        Owner = builder.Owner;
        TargetEntity = builder.TargetEntity;
        Damage = builder.Damage;
        Knockback = builder.Knockback;
        Stun = builder.Stun;
    }

    public override void Draw(GameTime gameTime)
    {
        
        base.Draw(gameTime);
    }
}