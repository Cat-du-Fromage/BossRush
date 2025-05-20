using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace BossRush.Entities;

public abstract class EntityBase : DrawableGameComponent
{
    protected BoundingBox BoundingBox;
    public Vector2 Position { get; private set;}
    protected Vector2 Velocity; // subclasses are in complete control of their acceleration
    
    public abstract bool IsAlive();
    
    /**
     * Inherits method from gameComponent. It will be called automatically by the game's Update method.
     * All subclasses overriding this should only update velocity and let this method handle the position
     * @param gameTime time elapsed since last call to Update
     */
    public override void Update(GameTime gameTime) => Position += (float)gameTime.ElapsedGameTime.TotalSeconds * Velocity;

    /**
     * Simply checks the two entities bounding boxes
     * @param other entity to check
     * @return whether this entity collides with other
     */
    public bool CollidesWith(EntityBase other)
    {
        return BoundingBox.Intersects(other.BoundingBox);
    }
    
    protected EntityBase(Vector2 position, Vector2 velocity, Game game):base(game)
    {
        Position = position;
        Velocity = velocity;
    }

    public abstract void Hit();
}