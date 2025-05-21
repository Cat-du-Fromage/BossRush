using System;
using System.Collections.Generic;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

namespace BossRush.Entities;

public class Projectile : EntityBase
{
    private bool _isAlive = true;
    public override bool IsAlive()
    {
        return _isAlive;
    }
    public float MaxSpeed { get; private set; }
    public float MaxAcceleration{ get; private set; }
    public float Friction { get; private set; }
    public Vector2 TargetPosition{ get; private set; }
    public int Size { get; private set; }
    
    public EntityBase Owner{ get; private set; }
    public EntityBase TargetEntity{ get; private set; }

    public float Damage{ get; private set; }
    public float Knockback{ get; private set; }
    public float Stun{ get; private set; }
    
    public Func<Projectile,Vector2> Direct { get;private set; }
    public Action<Projectile> OnDeath { get;private set; }
    
    public bool DiesOnImpact { get;private set; }
    
    private Timer _deathTimer;

    public class Builder
    {
        public Game Game { get; private set; }
        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; private set; } = Vector2.Zero;
        public float MaxSpeed { get; private set; }
        public int Size { get; private set; } = 15;
        public float MaxAcceleration{ get; private set; }
        public float Friction { get; private set; } = 1;
        public Vector2 TargetPosition{ get; private set; }
    
        public EntityBase Owner{ get; private set; }
        public EntityBase TargetEntity{ get; private set; }

        public float Damage{ get; private set; }
        public float Knockback{ get; private set; }
        public float Stun{ get; private set; }
        public Func<Projectile,Vector2> Direct { get;private set; }
        public Action<Projectile> OnDeath { get;private set; }
        public bool DiesOnImpact { get; private set; } = true;

        public TimeSpan LifeSpan { get; private set; } = TimeSpan.FromDays(1);
        
        public Builder SetDirect(Func<Projectile,Vector2> direct)
        {
                Direct =  direct;
                return this;
        }

        public Builder SetDeath(Action<Projectile> death)
        {
            OnDeath = death;
            return this;
        }

        public Builder SetMaxSpeed(float maxSpeed)
        {
            MaxSpeed = maxSpeed;
            return this;
        }

        public Builder SetMaxAcceleration(float maxAcceleration)
        {
            MaxAcceleration = maxAcceleration;
            return this;
        }

        public Builder SetFriction(float friction)
        {
            if(friction<0 || friction>1)
                throw new ArgumentException("Friction must be between 0 and 1");
            Friction = friction;
            return this;
        }

        public Builder SetStun(float stun)
        {
            Stun = stun;
            return this;
        }

        public Builder SetDamage(float damage)
        {
            Damage = damage;
            return this;
        }

        public Builder SetKnockback(float knockback)
        {
            Knockback =  knockback;
            return this;
        }

        public Builder SetOwner(EntityBase owner)
        {
            Owner = owner;
            return this;
        }

        public Builder SetTarget(EntityBase target)
        {
            TargetEntity =  target;
            return this;
        }

        public Builder SetTarget(Vector2 target)
        {
            TargetPosition =  target;
            return this;
        }

        public Builder SetGame(Game game)
        {
            Game = game;
            return this;
        }

        public Builder SetPosition(Vector2 position)
        {
            Position = position;
            return this;
        }

        public Builder SetVelocity(Vector2 velocity)
        {
            Velocity = velocity;
            return this;
        }

        public Builder SetSize(int size)
        {
            Size = size;
            return this;
        }

        public Builder SetLifeSpan(TimeSpan lifeSpan)
        {
            LifeSpan = lifeSpan;
            return this;
        }

        public Builder setDiesOnImpact(bool value)
        {
            DiesOnImpact = value;
            return this;
        }

        public Projectile Build()
        {
            return new Projectile(this);
        }
    }

    public static class Director
    {
        public static Builder MakeGuided(Builder builder)
        {
            builder.SetDirect(projectile =>
            {
                // If target is spinning, this projectile might follow it and circle endlessly
                // To stop this from happening we are going to cancel currentVelocity by a factor representing how close it is to being orthogonal to PT
                Vector2 pt = projectile.TargetEntity.Position - projectile.Position;
                pt.Normalize();
                
                Vector2 currentVelocity = projectile.Velocity;
                if (currentVelocity.LengthSquared() != 0)
                {
                    currentVelocity.Normalize();
                    pt -= currentVelocity * (1 - Math.Abs(Vector2.Dot(pt, currentVelocity)));
                }

                pt *= projectile.MaxAcceleration;
                return pt;
            });
            
            return builder;
        }
    }

    private Projectile(Builder builder) : base(builder.Position,builder.Velocity)
    {
        MaxSpeed = builder.MaxSpeed;
        MaxAcceleration = builder.MaxAcceleration;
        Friction = builder.Friction;
        TargetPosition = builder.TargetPosition;
        Owner = builder.Owner;
        TargetEntity = builder.TargetEntity;
        Damage = builder.Damage;
        Knockback = builder.Knockback;
        Stun = builder.Stun;
        Size = builder.Size;
        DiesOnImpact = builder.DiesOnImpact;
        
        OnDeath = builder.OnDeath;
        Direct = builder.Direct;
        
        _deathTimer = new Timer(builder.LifeSpan);
        _deathTimer.Elapsed += (_,_) => Hit(this); 
        _deathTimer.Start();

        BoundingBox = BoundingBox.CreateFromSphere(new BoundingSphere(new Vector3(Position.X,Position.Y,0), Size));
    }

    private void MovementUpdate(GameTime gameTime)
    {
        Velocity -= (float)gameTime.ElapsedGameTime.TotalSeconds * Friction * Velocity;
        if (Direct != null)
        {
            Vector2 acceleration = Direct(this) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (acceleration.Length() > MaxAcceleration)
            {
                acceleration.Normalize();
                acceleration *= MaxAcceleration;
            }
            Velocity += acceleration;
        }

        if (Velocity.Length() > MaxSpeed)
        {
            Velocity.Normalize();
            Velocity *= MaxSpeed;
        }
    }

    private void CollisionUpdate(GameTime gameTime)
    {
        BoundingBox = BoundingBox.CreateFromSphere(new BoundingSphere(new Vector3(Position.X,Position.Y,0), Size));
        
        // Hit stuff
        bool hit = false;
        List<Projectile> collidingProjectiles = FindAllColliding(ProjectileSystem.Instance.Projectiles);
        foreach (Projectile projectile in collidingProjectiles)
        {
            hit = true;
            projectile.Hit(this);
        }
        if((hit && DiesOnImpact) || TargetEntity != null && !TargetEntity.IsAlive()) // If target entity is dead, stop existing
            Hit(this);
    }

    public override void Update(GameTime gameTime)
    {
        MovementUpdate(gameTime);
        CollisionUpdate(gameTime);
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        SimpleShapes.Circle(Position,Size,Color.Red);
    }

    public override void Hit(EntityBase offender)
    {
        if (offender is Projectile projectile && projectile != this && !DiesOnImpact)
            return;
        _isAlive = false;
        ProjectileSystem.Remove(this);
        if(OnDeath != null)
            OnDeath(this);
    }
}