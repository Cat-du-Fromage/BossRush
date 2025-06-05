using System.Collections.Generic;
using BossRush.Enemy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BossRush.Entities;

/*
 Player will have 4 abilities:
 - baseAttack : shoots something towards a target position (mouse cursor for player)
 - baseDefense : spawns a (by default) homing projectile with zero speed
 - targetAttack : chooses the closest target close to position that is not the caster
 - distantMagic :  creates a projectile at position
 
 Abilities type define how the target is selected, the initial velocity and the initial position
 Abilities can be given properties that will be applied to the projectile builder
 The use method takes as argument the damage the projectile should yield
*/

public abstract class Ability 
{
 protected Projectile.Builder Builder;

 protected Ability()
 {
  Builder = new Projectile.Builder();
 }

 public Ability Apply(IProjectileDirector director)
 {
  director.Apply(Builder);
  return this;
 }

 public virtual void Use(EntityBase caster, Point target, float damage)
 {
  ProjectileSystem.Add(Builder.SetDamage(damage).Build());
 }

}


public class BaseAttack : Ability
{
 public override void Use(EntityBase caster, Point target, float damage)
 {
  Vector2 pt = target.ToVector2() - caster.Position;
  pt.Normalize();
  pt *= Builder.MaxSpeed;

  Builder
   .SetPosition(caster.Position)
   .SetVelocity(pt)
   .SetOwner(caster);
  
  base.Use(caster, target, damage);
 }
}

public class BaseDefense : Ability
{ public BaseDefense()
 {
  Apply(new Homing());
  Builder.SetSize(5)
   .SetVelocity(Vector2.Zero);
 }
 public override void Use(EntityBase caster, Point target, float damage)
 {
  // target the closest projectile from caster
  EntityBase closest = caster.FindClosestFromPoint(ProjectileSystem.Instance.Projectiles, caster.Position.ToPoint(), 200);
  if (closest == null)
   return;

  Builder.SetOwner(caster)
   .SetTarget(closest)
   .SetPosition(caster.Position);
  
  base.Use(caster, target, damage);
 }
}

public class TargetAttack : Ability
{

 public TargetAttack()
 {
  Builder.SetSize(20).SetTexture(Globals.ParticleTextures["magic"][2]);
 }
 public override void Use(EntityBase caster, Point target, float damage)
 {
  
  EntityBase closestEntity = caster.FindClosestFromPoint(
   Utility.Merge(ProjectileSystem.Instance.Projectiles,EnemySystem.Instance.Enemies),
   target,
   400);
  
  if (closestEntity == null)
   return;
  
  Builder.SetOwner(caster)
   .SetVelocity(Vector2.Zero)
   .SetMaxAcceleration(500)
   .SetMaxSpeed(400)
   .SetTarget(closestEntity)
   .SetPosition(caster.Position);
  
  base.Use(caster, target, damage);
 }
}

public class DistantMagic : Ability
{ 
 
 public DistantMagic()
 {
  Builder.SetSize(20).SetTexture(Globals.ParticleTextures["magic"][2]);
 }
 public override void Use(EntityBase caster, Point target, float damage)
 {
  Builder
   .SetPosition(target.ToVector2())
   .SetOwner(caster);
  
  base.Use(caster, target, damage);
 }
}