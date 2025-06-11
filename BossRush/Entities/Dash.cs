using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BossRush.Entities;
public class DashAbility
{
    public float DashDuration { get; set; } = 0.8f;
    public float DashSpeed { get; set; } = 700f;
    public float TransitionDuration { get; set; } = 0.1f; // Time for start/end animations
    
    public bool IsActive { get; private set; }
    public bool IsInvincible { get; private set; }
    public Vector2 Direction { get; private set; }
    
    private float _stateTimer;
    private DashState _currentState = DashState.Ready;
    
    public enum DashState
    {
        Ready,          // Can start dash
        Starting,      // Playing start animation
        Dashing,       // Moving with dash speed
        Ending,        // Playing end animation (reverse of start)
        Cooldown       // Optional cooldown period
    }
    
    public void Update(GameTime gameTime, Vector2 movementDirection, ref Vector2 velocity)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _stateTimer -= deltaTime;
        
        switch (_currentState)
        {
            case DashState.Ready:
                if (Keyboard.GetState().IsKeyDown(Keys.Space) && movementDirection != Vector2.Zero)
                {
                    StartDash(movementDirection);
                }
                break;
                
            case DashState.Starting:
                if (_stateTimer <= 0)
                {
                    TransitionToState(DashState.Dashing, DashDuration);
                }
                break;
                
            case DashState.Dashing:
                ApplyDashMovement(ref velocity);
                if (_stateTimer <= 0)
                {
                    TransitionToState(DashState.Ending, TransitionDuration);
                }
                break;
                
            case DashState.Ending:
                if (_stateTimer <= 0)
                {
                    EndDash(ref velocity);
                }
                break;
            case DashState.Cooldown:
                if (_stateTimer <= 0)
                {
                    TransitionToState(DashState.Ready, 0);
                }
                break;
        }
        
        UpdateInvincibility();
    }
    
    private void StartDash(Vector2 direction)
    {
        IsActive = true;
        IsInvincible = true;
        Direction = Vector2.Normalize(direction);
        TransitionToState(DashState.Starting, TransitionDuration);
    }
    
    private void TransitionToState(DashState newState, float duration)
    {
        _currentState = newState;
        _stateTimer = duration;
        
        // Reset invincibility when starting the dash proper
        if (newState == DashState.Dashing)
        {
            IsInvincible = true;
        }
    }
    
    private void ApplyDashMovement(ref Vector2 velocity)
    {
        // Smooth curve using sine wave (peaks at midpoint)
        float progress = _stateTimer / DashDuration;
        float speed = DashSpeed * (float)Math.Sin(progress * Math.PI);
        velocity = Direction * speed;
    }
    
    private void EndDash(ref Vector2 velocity)
    {
        IsActive = false;
        velocity = Vector2.Zero;
        TransitionToState(DashState.Ready, 0);
    }
    
    private void UpdateInvincibility()
    {
        // Remain invincible during entire dash sequence
        IsInvincible = _currentState == DashState.Starting || 
                      _currentState == DashState.Dashing || 
                      _currentState == DashState.Ending;
    }
    
    public bool ShouldPlayStartAnimation() => _currentState == DashState.Starting;
    public bool ShouldPlayEndAnimation() => _currentState == DashState.Ending;
    
    public void Reset()
    {
        IsActive = false;
        IsInvincible = false;
        _currentState = DashState.Ready;
        _stateTimer = 0;
    }
}