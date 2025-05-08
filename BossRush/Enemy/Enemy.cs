using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Enemy;

public class Enemy
{
    public string Name { get; private set; }
    public int BaseHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    public float MoveSpeed { get; private set; }
    
    public float Range { get; private set; }
    
//╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
//║                                             ◆◆◆◆◆◆ CONSTRUCTOR ◆◆◆◆◆◆                                              ║
//╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝

    private Enemy() { }
    
//╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
//║                                           ◆◆◆◆◆◆ MONOGAME EVENTS ◆◆◆◆◆◆                                            ║
//╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝
    public void Update(GameTime gameTime/*, Vector2 playerPosition*/)
    {
        // Si a porté => tirer
        // Sinon => Avancer
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        //Sprite.Draw(spriteBatch, Transform);
    }
//╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
//║                                            ◆◆◆◆◆◆ CLASS METHODS ◆◆◆◆◆◆                                             ║
//╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝

    public void Move(Vector2 playerPosition)
    {
        //1) prendre la Position du joueur
        //2) Translation ou rotation
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
        private readonly Enemy enemy = new Enemy();
        
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
}