using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BossRush.Entities;

public abstract class EntityBase
{
    protected BoundingBox BoundingBox;
    public Vector2 Position { get; private set;}
    protected Vector2 Velocity; // subclasses are in complete control of their acceleration
    
    public Vector2 GetVelocity(){
        return Velocity;
    }
    
    public abstract bool IsAlive();
    
    /**
     * Inherits method from gameComponent. It will be called automatically by the game's Update method.
     * All subclasses overriding this should only update velocity and let this method handle the position
     * @param gameTime time elapsed since last call to Update
     */
    public virtual void Update(GameTime gameTime) => Position += (float)gameTime.ElapsedGameTime.TotalSeconds * Velocity;

    public abstract void Draw(SpriteBatch spriteBatch);

    /**
     * Simply checks the two entities bounding boxes
     * @param other entity to check
     * @return whether this entity collides with other
     */
    public bool CollidesWith(EntityBase other)
    {
        if (other == this)
            return false; // entity doesn't collide with itself
        return BoundingBox.Intersects(other.BoundingBox);
    }

    public List<T> FindAllColliding<T> (IReadOnlyCollection<T> others) where T : EntityBase
    {
        List<T> colliding = new List<T>();
        foreach (var entity in others)
        { 
            if (CollidesWith(entity))
                colliding.Add(entity);
        }
        return colliding;
    }
    
    public T FindClosestFromPoint<T>(IReadOnlyCollection<T> others, Point target, float range) where T : EntityBase
    {
        T closest = null;
        float closestSquaredDistance = float.MaxValue;
        foreach (T entity in others)
        {
            if(entity == this)
                continue;
            Vector2 ct = entity.Position - target.ToVector2();
            if (ct.LengthSquared() < closestSquaredDistance)
            {
                closestSquaredDistance = ct.LengthSquared();
                closest = entity;
            }
        }
        
        // Ensure it is within reasonable distance
        if (closestSquaredDistance > range * range)
            return null;
        
        return closest;
    }
    
    protected EntityBase(Vector2 position, Vector2 velocity)
    {
        Position = position;
        Velocity = velocity;
    }

    public abstract void Hit(EntityBase offender);
}

