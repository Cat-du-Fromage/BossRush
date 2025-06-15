using System;
using System.Collections.Generic;
using BossRush.Entities;
using BossRush.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Enemy;

public class Enemy : EntityBase
{
    private Texture2D texture;
    /**
     * The color used to render this enemy
     */
    public Color Color { get; private set; } = Color.Purple;
    
    /**
     * The name identifier for this enemy type
     */
    public string Name { get; private set; }
    
    /**
     * The size (width and height) of this enemy
     */
    public float Size { get; private set; }
    
    /**
     * Whether this enemy is a melee type
     */
    public bool IsMelee { get; private set; }
    
    /**
     * The base health value for this enemy
     */
    public int BaseHealth { get; private set; }
    
    /**
     * The movement speed of this enemy
     */
    public float MoveSpeed { get; private set; }
    
    /**
     * The damage dealt by this enemy's attacks
     */
    public int Damage { get; private set; }
    
    /**
     * The cooldown manager for this enemy's attacks
     */
    public AttackCooldown AttackCooldown { get; private set; }
    
    /**
     * The special ability of this enemy
     */
    public Ability Ability { get; private set; }
    
    /**
     * The attack range of this enemy (0 for melee)
     */
    public float Range { get; private set; }
    
    /**
     * Checks if the enemy is currently alive
     * @return true if current health > 0, false otherwise
     */
    public override bool IsAlive() => CurrentHealth > 0;
    
//╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
//║                                             ◆◆◆◆◆◆ CONSTRUCTOR ◆◆◆◆◆◆                                              ║
//╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝

    /**
     * Protected constructor for Enemy class
     * @param position Initial position vector
     * @param velocity Initial velocity vector
     */
    protected Enemy(Vector2 position, Vector2 velocity) : base(position, velocity)
    {
        texture = Globals.Content.Load<Texture2D>("triangleRed");
    }
    
//╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
//║                                           ◆◆◆◆◆◆ MONOGAME EVENTS ◆◆◆◆◆◆                                            ║
//╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝

    /**
     * Updates the enemy state each frame
     * @param gameTime Current game timing snapshot
     * @param playerPosition Current position of the player
     */
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
        BoundingBox = BoundingBox.CreateFromSphere(new BoundingSphere(new Vector3(Position.X,Position.Y,0), Size/2));
    }

    /**
     * Prevents this enemy from overlapping with other enemies
     * @param allEnemies List of all active enemies to check against
     */
    public void AvoidOverlap(List<Enemy> allEnemies)
    {
        float minDistance = Size * 2; // Distance minimale souhaitée
        const float repulsionForce = 0.5f; // Force de répulsion

        foreach (Enemy other in allEnemies)
        {
            if (other == this) continue;
            Vector2 direction = Position - other.Position;
            float distance = direction.Length();

            // Si trop proche
            if (distance < minDistance && distance > 0)
            {
                direction.Normalize();
                Velocity += direction * repulsionForce * (minDistance - distance);
            }
        }
    }

    /**
     * Draws the enemy to the screen
     * @param spriteBatch The SpriteBatch used for rendering
     */
    public override void Draw(SpriteBatch spriteBatch)
    {
        SimpleShapes.Rectangle(Position, Vector2.One * Size, Color);
    }
    
//╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
//║                                            ◆◆◆◆◆◆ CLASS METHODS ◆◆◆◆◆◆                                             ║
//╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝

    /**
     * Moves the enemy toward a specified direction
     * @param directionToPlayer Normalized vector pointing toward player
     */
    public void Move(Vector2 directionToPlayer)
    {
        float maxDistance = IsMelee ? 0 : 150;
        if (Vector2.Distance(Player.Instance.Position, Position) <= maxDistance)
        {
            Velocity = Vector2.Zero;
        }
        else
        {
            Velocity = directionToPlayer * MoveSpeed;
            AvoidOverlap(EnemySystem.Instance.GetNearbyEnemies(Position));
        }
    }

    /**
     * Performs an attack in the specified direction
     * @param directionToPlayer Normalized vector pointing toward player
     */
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
    
    /**
     * Handles damage taken by this enemy
     * @param offender The entity that caused the damage
     */
    public override void Hit(EntityBase offender)
    {
        if (offender is not Projectile projectile) return;
        if (projectile.Owner is Enemy) return;
        CurrentHealth -= (int)projectile.Damage; //projectile.Damage;
    }

//-====================================================================================================================-
//╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
//║                                               ◆◆◆◆◆◆ BUILDER ◆◆◆◆◆◆                                                ║
//╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝
//-====================================================================================================================-
    
    /**
     * Builder pattern implementation for constructing Enemy instances
     */
    public class Builder
    {
        private readonly Enemy enemy;

        /**
         * Creates a new Enemy builder
         * @param position Initial position
         * @param velocity Initial velocity
         */
        public Builder(Vector2 position, Vector2 velocity) => enemy = new Enemy(position, velocity);
        
        /**
         * Sets the enemy name
         * @param name Enemy name
         * @return Builder instance for chaining
         */
        public Builder WithName(string name)
        {
            enemy.Name = name;
            return this;
        }
        
        /**
         * Sets the enemy color
         * @param color Render color
         * @return Builder instance for chaining
         */
        public Builder WithColor(Color color)
        {
            enemy.Color = color;
            return this;
        }
        
        /**
         * Sets the enemy size
         * @param enemy size
         * @return Builder instance for chaining
         */
        public Builder WithSize(float size)
        {
            enemy.Size = size;
            return this;
        }
        
        /**
         * Sets the enemy damage
         * @param enemy damage
         * @return Builder instance for chaining
         */
        public Builder WithDamage(int damage)
        {
            enemy.Damage = damage;
            return this;
        }
        
        /**
         * Sets the enemy as melee
         * @param is enemy melee ?
         * @return Builder instance for chaining
         */
        public Builder IsMelee(bool state)
        {
            enemy.IsMelee = state;
            return this;
        }
        
        /**
         * Sets the enemy health
         * @param enemy health
         * @return Builder instance for chaining
         */
        public Builder WithHealth(int health)
        {
            enemy.BaseHealth = health;
            enemy.CurrentHealth = health;
            return this;
        }

        /**
         * Sets the enemy speed
         * @param enemy speed
         * @return Builder instance for chaining
         */
        public Builder WithMoveSpeed(float moveSpeed)
        {
            enemy.MoveSpeed = moveSpeed;
            return this;
        }

        /**
         * Sets the enemy range
         * @param enemy range
         * @return Builder instance for chaining
         */
        public Builder WithRange(float range)
        {
            enemy.Range = range;
            return this;
        }
        
        /**
         * Sets the enemy attack speed
         * @param enemy attack speed
         * @return Builder instance for chaining
         */
        public Builder WithAttackCooldown(float cooldown)
        {
            enemy.AttackCooldown = new AttackCooldown(cooldown);
            return this;
        }
        
        /**
         * Sets the enemy range attack ability
         * @param enemy range attack ability
         * @return Builder instance for chaining
         */
        public Builder WithAbility(Ability ability)
        {
            enemy.Ability = ability;
            return this;
        }

        /**
         * Validate value to create the enemy
         */
        private void Validate()
        {
            if (string.IsNullOrEmpty(enemy.Name)) enemy.Name = "DefaultName";
            if (enemy.IsMelee) enemy.Range = 0;
            if (enemy.Damage < 0) enemy.Damage = 0;
            if (enemy.MoveSpeed < 0) enemy.MoveSpeed = 10;
            if (enemy.Size <= 0) enemy.Size = 16;
            if (enemy.BaseHealth <= 0)
            {
                enemy.BaseHealth = 10;
                enemy.CurrentHealth = 10;
            }
            if (!enemy.IsMelee)
            {
                if (enemy.Range <= 0)
                    enemy.Range = 10;
                if(enemy.Ability == null)
                    enemy.Ability = new BaseAttack().Apply(new Arrow());
            }
        }

        /**
         * Constructs the final Enemy instance
         * @return The built Enemy object
         */
        public Enemy Build()
        {
            Validate();
            return enemy;
        }
    }
}