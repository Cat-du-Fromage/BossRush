using Microsoft.Xna.Framework;

namespace BossRush.Entities;

public abstract class EntityBase : DrawableGameComponent
{
    private BoundingBox _boundingBox;
    private Vector2 _position;
    protected Vector2 Velocity; // subclasses are in complete control of their acceleration
    
    public abstract bool IsAlive();
    
    // Position getter
    public Vector2 Position() => _position;
    
    /**
     * Inherits method from gameComponent. It will be called automatically by the game's Update method.
     * All subclasses overriding this should only update velocity and let this method handle the position
     * @param gameTime time elapsed since last call to Update
     */
    public override void Update(GameTime gameTime) => _position += (float)gameTime.ElapsedGameTime.TotalSeconds * Velocity;

    /**
     * Simply checks the two entities bounding boxes
     * @param other entity to check
     * @return whether this entity collides with other
     */
    public bool CollidesWith(EntityBase other)
    {
        return _boundingBox.Intersects(other._boundingBox);
    }
    
    protected EntityBase(Vector2 position, Vector2 velocity, Game game):base(game)
    {
        _position = position;
        Velocity = velocity;
    }
}