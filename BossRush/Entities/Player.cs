using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BossRush.Entities;

public class Player : EntityBase
{ 
    
    public static Player Instance{get; private set;}

    public static void Initialize(Vector2 position)
    {
        if (Instance != null)
            throw new ConstraintException("Only one player instance");
        
        Instance = new Player(position);
    }
    private Player(Vector2 position) : base(position, Vector2.Zero)
    {
        BoundingBox = BoundingBox.CreateFromPoints(new List<Vector3>([new Vector3(Position.X,Position.Y,10), new Vector3(Position.X+32,Position.Y+32,-10)]),0,2);
        
        _abilities.Add(new PlayerAbility(
            new BaseAttack().Apply(new Arrow()).Apply(new Explosive()),Keys.D1));
        
        _abilities.Add(new PlayerAbility(new BaseDefense(),Keys.D2));
        
        _abilities.Add(new PlayerAbility(
            new TargetAttack().Apply(new Homing()),Keys.D3));
        
        _abilities.Add(new PlayerAbility(
            new DistantMagic()
                .Apply(new MoveToCaster())
                .Apply(new DangerZone(TimeSpan.FromSeconds(3))),
            Keys.D4));
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        SimpleShapes.Rectangle(Position,30 * Vector2.One,Color.Brown);
        
    }
    
    private class PlayerAbility(Ability ability, Keys key)
    {
        private Ability ability = ability;
        private Keys key = key;
        private bool wasDown;
        public void Update(Player caster)
        {
            if (Keyboard.GetState().IsKeyDown(key) && !wasDown)
            {
                wasDown = true;
                ability.Use(caster,Mouse.GetState().Position);
            }
            else if (Keyboard.GetState().IsKeyUp(key) && wasDown)
                wasDown = false;
        }
    }
    
    private List<PlayerAbility> _abilities = new List<PlayerAbility>();

    private void UseAbilities()
    {
        foreach (var ability in _abilities)
            ability.Update(this);
    }

    public override void Update(GameTime gameTime)
    {
        UseAbilities();
        
        const float acceleration = 200, baseSpeed = 100, stopThreshold = 10;
        Vector2 direction = Vector2.Zero;
        if(Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D))
            direction += Vector2.UnitX;
        if(Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.A))
            direction -= Vector2.UnitX;
        if(Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W))
            direction -= Vector2.UnitY;
        if(Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S))
            direction += Vector2.UnitY;

        if (direction.LengthSquared() > 0)
        {
            direction.Normalize(); // to make sure we cannot go faster in diagonal
            direction *= acceleration;
        }
        else if (Velocity.LengthSquared() > 0)
        {
            direction = -Velocity;
            if (direction.Length() > acceleration)
            {
                direction.Normalize();
                direction *= acceleration;
            }
        }

        Velocity += direction * (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (Velocity.Length() > baseSpeed)
        {
            Velocity.Normalize();
            Velocity *= baseSpeed;
        }
        if(Velocity.LengthSquared() < stopThreshold)
            Velocity = Vector2.Zero;
        
        BoundingBox = BoundingBox.CreateFromPoints(new List<Vector3>([new Vector3(Position.X,Position.Y,10), new Vector3(Position.X+32,Position.Y+32,-10)]),0,2);
        
        base.Update(gameTime);
    }

    public override bool IsAlive()
    {
        return true;
    }

    public override void Hit(EntityBase offender)
    {
        Console.Out.WriteLine("You got hit");
    }
}