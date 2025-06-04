using System;
using BossRush.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Enemy;

public class Enemy : EntityBase
{
    private Texture2D texture;
    
    public string Name { get; private set; }
    public float Size { get; private set; }
    public int BaseHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    public float MoveSpeed { get; private set; }
    
    public float Range { get; private set; }

    public bool IsMelee => Range < 10;
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
        if (Vector2.Distance(Position, playerPosition) <= Range)
        {
            Attack(direction);
        }
        else // Sinon => Avancer
        {
            Move(direction);
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
        //1) prendre la Position du joueur
        Velocity = directionToPlayer * MoveSpeed;
    }

    public void Attack(Vector2 directionToPlayer)
    {
        Velocity = Vector2.Zero;
        //1) prendre la direction
        //2) créer un projectile
    }
    
    public override void Hit(EntityBase offender)
    {
        if (offender is not Projectile projectile) return;
        CurrentHealth -= 10; //projectile.Damage;
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