using System;
using BossRush.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Enemy;

public class Enemy : EntityBase
{
    private Texture2D texture;
    
    public string Name { get; private set; }
    public int BaseHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    public float MoveSpeed { get; private set; }
    
    public float Range { get; private set; }
    
    
    public override bool IsAlive() => CurrentHealth > 0;
    
//╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
//║                                             ◆◆◆◆◆◆ CONSTRUCTOR ◆◆◆◆◆◆                                              ║
//╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝

    protected Enemy(Vector2 position, Vector2 velocity, Game game) : base(position, velocity)
    {
        texture = game.Content.Load<Texture2D>("triangleRed");
    }

    private Enemy(Game game) : this(Vector2.Zero, Vector2.Zero,game) { }
    
//╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
//║                                           ◆◆◆◆◆◆ MONOGAME EVENTS ◆◆◆◆◆◆                                            ║
//╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝

    public void OnUpdate(GameTime gameTime, Vector2 playerPosition)
    {
        // Si a porté => tirer
        // Sinon => Avancer
        //double deltaTime = gameTime.ElapsedGameTime.TotalMilliseconds;
        Move(playerPosition);
        Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture,
            Position,
            null,
            Color.White,
            0f,
            new Vector2(texture.Width / 2, texture.Height / 2),
            new Vector2(0.005f, 0.005f),
            SpriteEffects.None,
            0f);
    }
    
//╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
//║                                            ◆◆◆◆◆◆ CLASS METHODS ◆◆◆◆◆◆                                             ║
//╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝

    public void Move(Vector2 playerPosition)
    {
        //1) prendre la Position du joueur
        Vector2 direction = (playerPosition - Position);
        direction.Normalize();
        Velocity = direction * MoveSpeed;
        
        //TODO faire la rotation
    }

    public void Attack()
    {
        //1) prendre la direction
        //2) créer un projectile
    }

//-====================================================================================================================-
//╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
//║                                               ◆◆◆◆◆◆ BUILDER ◆◆◆◆◆◆                                                ║
//╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝
//-====================================================================================================================-
    public class Builder
    {
        private readonly Enemy enemy;

        public Builder(Vector2 position, Vector2 velocity, Game game) => enemy = new Enemy(position, velocity, game);
        
        public Builder WithName(string name)
        {
            enemy.Name = name;
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

    public override void Hit(EntityBase offender)
    {
        throw new NotImplementedException();
    }
}