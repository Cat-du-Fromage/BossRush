using System;
using BossRush.Entities;
using BossRush.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Enemy;

public class Enemy : EntityBase
{
    private Texture2D texture;
    
    public string Name { get; private set; }
    public float Size { get; private set; }
    
    public bool IsMelee { get; private set; }
    public int BaseHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    public float MoveSpeed { get; private set; }
    public int Damage { get; private set; }
    public AttackCooldown AttackCooldown { get; private set; }
    public Ability Ability { get; private set; }
    
    public float Range { get; private set; }
    
    public override bool IsAlive() => CurrentHealth > 0;
    
//╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
//║                                             ◆◆◆◆◆◆ CONSTRUCTOR ◆◆◆◆◆◆                                              ║
//╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝

    protected Enemy(Vector2 position, Vector2 velocity) : base(position, velocity)
    {
        texture = Globals.Content.Load<Texture2D>("triangleRed");
    }

    private Enemy() : this(Vector2.Zero, Vector2.Zero) { }
    
//╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
//║                                           ◆◆◆◆◆◆ MONOGAME EVENTS ◆◆◆◆◆◆                                            ║
//╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝

    public void OnUpdate(GameTime gameTime, Vector2 playerPosition)
    {
        Vector2 direction = (playerPosition - Position);
        direction.Normalize();
        // Si a porté => tirer
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (AttackCooldown.CanAttack() && Vector2.Distance(Position, playerPosition) <= (IsMelee ? Size + 32 : Range))
        {
            Attack(direction);
            AttackCooldown.SetCooldown();
        }
        else // Sinon => Avancer
        {
            Move(direction);
            AttackCooldown.Update(deltaTime);
        }
        Update(gameTime);
        BoundingBox = BoundingBox.CreateFromSphere(new BoundingSphere(new Vector3(Position.X,Position.Y,0), 16));
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        SimpleShapes.Rectangle(Position, Vector2.One * Size, Color.Purple);
        /*
        spriteBatch.Draw(texture,
            Position,
            null,
            Color.White,
            0f,
            new Vector2(texture.Width / 2, texture.Height / 2),
            new Vector2(0.005f, 0.005f),
            SpriteEffects.None,
            0f);
            */
    }
    
//╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
//║                                            ◆◆◆◆◆◆ CLASS METHODS ◆◆◆◆◆◆                                             ║
//╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝

    public void Move(Vector2 directionToPlayer)
    {
        float maxDistance = IsMelee ? 0 : 100;
        if (Vector2.Distance(Player.Instance.Position, Position) <= maxDistance)
        {
            Velocity = Vector2.Zero;
        }
        else
        {
            Velocity = directionToPlayer * MoveSpeed;
        }
    }

    public void Attack(Vector2 directionToPlayer)
    {
        if (IsMelee)
        {
            Vector2 midPosition = (Player.Instance.Position + Position) / 2;
            ParticleSystem.Instance.Presets.CreateSlashEffect(midPosition, directionToPlayer);
            Player.Instance.Hit(this);
        }
        else
        {
            Ability.Use(this, Player.Instance.Position.ToPoint(), Damage);
        }
    }
    
    public override void Hit(EntityBase offender)
    {
        if (offender is not Projectile projectile) return;
        CurrentHealth -= (int)projectile.Damage; //projectile.Damage;
    }

//-====================================================================================================================-
//╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
//║                                               ◆◆◆◆◆◆ BUILDER ◆◆◆◆◆◆                                                ║
//╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝
//-====================================================================================================================-
    public class Builder
    {
        private readonly Enemy enemy;

        public Builder(Vector2 position, Vector2 velocity) => enemy = new Enemy(position, velocity);
        
        public Builder WithName(string name)
        {
            enemy.Name = name;
            return this;
        }
        
        public Builder WithSize(float size)
        {
            enemy.Size = size;
            return this;
        }
        
        public Builder WithDamage(int damage)
        {
            enemy.Damage = damage;
            return this;
        }
        
        public Builder IsMelee(bool state)
        {
            enemy.IsMelee = state;
            return this;
        }
        
        public Builder WithHealth(int health)
        {
            enemy.BaseHealth = health;
            enemy.CurrentHealth = health;
            return this;
        }

        public Builder WithMoveSpeed(float moveSpeed)
        {
            enemy.MoveSpeed = moveSpeed;
            return this;
        }

        //TODO: Add Projectile check
        public Builder WithRange(float range)
        {
            enemy.Range = range;
            return this;
        }
        
        public Builder WithAttackCooldown(float cooldown)
        {
            enemy.AttackCooldown = new AttackCooldown(cooldown);
            return this;
        }
        
        //TODO ADD CHECK IF RANGE
        public Builder WithAbility(Ability ability)
        {
            enemy.Ability = ability;
            return this;
        }


        public Enemy Build()
        {
            // Validation des champs obligatoires
            if (string.IsNullOrEmpty(enemy.Name))
            {
                throw new InvalidOperationException("Name is required to build Enemy");
            }
            return enemy;
        }
    }
}