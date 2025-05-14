using System;
using Microsoft.Xna.Framework;

namespace BossRush.Entities;

public class Projectile : EntityBase
{
    private float _maxSpeed;
    private float _maxAcceleration;
    private Vector2 targetPosition;
    
    private EntityBase _owner;
    private EntityBase _targetEntity;

    private float _damage;
    private float _knockback;
    private float _stun;
    
    
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

    private IDirect _direct;
    private IDeath _death;

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
        _maxSpeed =  builder.MaxSpeed;
        _maxAcceleration =  builder.MaxAcceleration;
        targetPosition =  builder.TargetPosition;
        _owner =  builder.Owner;
        _targetEntity = builder.TargetEntity;
        _damage =  builder.Damage;
        _knockback =   builder.Knockback;
        _stun =   builder.Stun;
    }
}