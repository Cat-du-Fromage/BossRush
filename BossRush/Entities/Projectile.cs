using System;
using System.Collections.Generic;
using System.Timers;
using BossRush.Enemy;
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
    
    public Func<Projectile,Vector2> Direct { get;private set; }
    public Action<Projectile> OnDeath { get;private set; }
    public Action<Projectile> OnHit{ get;private set; }
    public Action<Projectile> OnUpdate{ get;private set; }
    public Action<Projectile> OnFire{ get;private set; }
    
    public int ImpactResistance { get; private set; }

    private DateTime _deathTime;
    private Texture2D _texture;
    private Color _color;

    public class Builder
    {
        public Vector2 Position { get; private set; } = Vector2.Zero;
        public Vector2 Velocity { get; private set; } = Vector2.Zero;
        public float MaxSpeed { get; private set; } = 1000;
        public int Size { get; private set; } = 15;
        public float MaxAcceleration { get; private set; } = 100;
        public float Friction { get; private set; } = 1;
        public Vector2 TargetPosition{ get; private set; }
    
        public EntityBase Owner{ get; private set; }
        public EntityBase TargetEntity{ get; private set; }

        public float Damage{ get; private set; }
        public Func<Projectile,Vector2> Direct { get;private set; }
        public Action<Projectile> OnDeath { get;private set; }
        public Action<Projectile> OnHit{ get;private set; }
        public Action<Projectile> OnUpdate{ get;private set; }
        public Action<Projectile> OnFire{ get;private set; }
        public int ImpactResistance { get; private set; } = 0;

        public TimeSpan LifeSpan { get; private set; } = TimeSpan.FromDays(1);
        public Texture2D ProjectileTexture { get; private set; } = null;
        public Color Color { get; private set; } = Color.Red;

        public Builder Clone()
        {
            return (Builder)MemberwiseClone();
        } 
        
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
        
        public Builder SetOnUpdate(Action<Projectile> update)
        {
            OnUpdate = update;
            return this;
        }
        
        public Builder SetOnHit(Action<Projectile> onHit)
        {
            OnHit = onHit;
            return this;
        }
        
        public Builder SetOnFire(Action<Projectile> onFire)
        {
            OnFire = onFire;
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

        public Builder SetDamage(float damage)
        {
            Damage = damage;
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

        public Builder SetImpactResistance(int value)
        {
            if (value < 0)
            {
                throw new ArgumentException("Impact resistance must be positive");
            }
            ImpactResistance = value;
            return this;
        }

        public Builder SetColor(Color color)
        {
            Color = color;
            return this;
        }

        public Builder SetTexture(Texture2D texture)
        {
            ProjectileTexture = texture;
            return this;
        }

        public Projectile Build()
        {
            return new Projectile(this);
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
        Size = builder.Size;
        ImpactResistance = builder.ImpactResistance;
        _texture = builder.ProjectileTexture;
        _color = builder.Color;
        
        OnDeath = builder.OnDeath;
        OnHit = builder.OnHit;
        OnUpdate = builder.OnUpdate;
        OnFire = builder.OnFire;
        Direct = builder.Direct;

        _deathTime = DateTime.Now + builder.LifeSpan;

        BoundingBox = BoundingBox.CreateFromSphere(new BoundingSphere(new Vector3(Position.X,Position.Y,0), Size));
        
        OnFire?.Invoke(this);
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
        
        // Collide with other projectiles
        List<Projectile> collidingProjectiles = FindAllColliding(ProjectileSystem.Instance.Projectiles);
        foreach (Projectile projectile in collidingProjectiles)
        {
            projectile.Hit(this);
        }
        if((collidingProjectiles.Count > 0))
            Hit(this);
        if(TargetEntity != null && !TargetEntity.IsAlive())
            Die();

        // Collide with enemies
        var collidingEnemies = FindAllColliding(EnemySystem.Instance.Enemies);
        foreach (Enemy.Enemy enemy in collidingEnemies)
        {
            if(enemy != Owner)
                enemy.Hit(this);
        }
        if(collidingEnemies.Count > 0)
            Hit(this);
        
        // TODO : Collide with player
        
    }

    public override void Update(GameTime gameTime)
    {
        if (!IsAlive())
            return;
        
        if (DateTime.Now > _deathTime)
        {
            Die();
        }
        
        OnUpdate?.Invoke(this);
        
        MovementUpdate(gameTime);
        CollisionUpdate(gameTime);
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (_texture != null)
        {
            var rotation = Velocity.X == 0 ? 0 : (float)(Math.Atan(Velocity.Y / Velocity.X) + Math.PI / 2);
            float scale = 2*Size / (float)Math.Min(_texture.Width, _texture.Height);
            spriteBatch.Draw(
                texture: _texture,
                position: Position,
                sourceRectangle: null,
                color: _color,
                rotation: rotation,
                origin: new Vector2(_texture.Width / 2, _texture.Height / 2),
                scale: scale,
                effects: SpriteEffects.None,
                layerDepth: 0
            );
        }
        else
        {
            SimpleShapes.Circle(Position, Size, _color);
        }
    }

    private void Die()
    {
        _isAlive = false;
        ProjectileSystem.Remove(this);
        OnDeath?.Invoke(this);
    }

    public override void Hit(EntityBase offender)
    {
        OnHit?.Invoke(this);
        if (ImpactResistance <= 0)
        {
            Die();
        }
        else
        {
            --ImpactResistance;
        }
    }
}