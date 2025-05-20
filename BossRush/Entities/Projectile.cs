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
    public float MaxSpeed { get; private set; }
    public float MaxAcceleration{ get; private set; }
    public float Friction { get; private set; }
    public Vector2 TargetPosition{ get; private set; }
    
    public EntityBase Owner{ get; private set; }
    public EntityBase TargetEntity{ get; private set; }

    public float Damage{ get; private set; }
    public float Knockback{ get; private set; }
    public float Stun{ get; private set; }
    
    public Func<Projectile,Vector2> Direct { get;private set; }
    public Action<Projectile> OnDeath { get;private set; } 

    public class Builder
    {
        public Game Game { get; private set; }
        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; private set; }
        public float MaxSpeed { get; private set; }
        public float MaxAcceleration{ get; private set; }
        public float Friction { get; private set; }
        public Vector2 TargetPosition{ get; private set; }
    
        public EntityBase Owner{ get; private set; }
        public EntityBase TargetEntity{ get; private set; }

        public float Damage{ get; private set; }
        public float Knockback{ get; private set; }
        public float Stun{ get; private set; }
        public Func<Projectile,Vector2> Direct { get;private set; }
        public Action<Projectile> OnDeath { get;private set; } 
        
        public Builder setDirect(Func<Projectile,Vector2> direct)
        {
                Direct =  direct;
                return this;
        }

        public Builder setDeath(Action<Projectile> death)
        {
            OnDeath = death;
            return this;
        }

        public Builder setMaxSpeed(float maxSpeed)
        {
            MaxSpeed = maxSpeed;
            return this;
        }

        public Builder setMaxAcceleration(float maxAcceleration)
        {
            MaxAcceleration = maxAcceleration;
            return this;
        }

        public Builder setFriction(float friction)
        {
            if(friction<0 || friction>1)
                throw new ArgumentException("Friction must be between 0 and 1");
            Friction = friction;
            return this;
        }

        public Builder setStun(float stun)
        {
            Stun = stun;
            return this;
        }

        public Builder setDamage(float damage)
        {
            Damage = damage;
            return this;
        }

        public Builder setKnockback(float knockback)
        {
            Knockback =  knockback;
            return this;
        }

        public Builder setOwner(EntityBase owner)
        {
            Owner = owner;
            return this;
        }

        public Builder setTarget(EntityBase target)
        {
            TargetEntity =  target;
            return this;
        }

        public Builder setTarget(Vector2 target)
        {
            TargetPosition =  target;
            return this;
        }

        public Builder setGame(Game game)
        {
            Game = game;
            return this;
        }

        public Builder setPosition(Vector2 position)
        {
            Position = position;
            return this;
        }

        public Builder setVelocity(Vector2 velocity)
        {
            Velocity = velocity;
            return this;
        }

        public Projectile build()
        {
            return new Projectile(this);
        }
    }

    public class Director
    {
        public static Builder MakeGuided(Builder builder)
        {
            builder.setDirect(projectile =>
            {
                // If target is spinning, this projectile might follow it and circle endlessly
                // To stop this from happening we are going to cancel currentVelocity by a factor representing how close it is to being orthogonal to PT
                Vector2 PT = projectile.TargetEntity.Position - projectile.Position;
                PT.Normalize();
                
                Vector2 currentVelocity = projectile.Velocity;
                if (currentVelocity.LengthSquared() != 0)
                {
                    currentVelocity.Normalize();
                    PT -= currentVelocity * (1 - Math.Abs(Vector2.Dot(PT, currentVelocity)));
                }

                PT *= projectile.MaxAcceleration;
                return PT;
            });
            
            return builder;
        }
    }

    private Projectile(Builder builder) : base(builder.Position,builder.Velocity,builder.Game)
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
        
        OnDeath = builder.OnDeath;
        Direct = builder.Direct;

        BoundingBox = BoundingBox.CreateFromSphere(new BoundingSphere(new Vector3(Position.X,Position.Y,0), 15));
    }

    public override void Update(GameTime gameTime)
    {
        // Movement
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
        
        BoundingBox = BoundingBox.CreateFromSphere(new BoundingSphere(new Vector3(Position.X,Position.Y,0), 12));
        
        // Hit stuff
        bool hit = false;
        foreach (var component in Game.Components)
        {
            if (component is EntityBase @base)
            {
                if(@base == this)
                    continue;
                if (CollidesWith(@base))
                {
                    @base.Hit();
                    hit = true;
                }
            }
        }
        if(hit)
            Hit();
        
        
        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        SimpleShapes.Circle(Position,15,Color.Red);
        base.Draw(gameTime);
    }

    public override void Hit()
    {
        //Game.Components.Remove(this);
        Velocity = Vector2.Zero;
    }
}