// ================================================================================
// File : Dash.cs
// Project name : BossRush
// Project members :
// - Florian Duruz, Mathieu Rabot, Raphaël Perret
// ================================================================================

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BossRush.Entities;

/**
 * @brief Represents a dash ability for a character.
 * @details This class manages the dash state, including starting, dashing, and ending the dash.
 * It also handles invincibility during the dash and smooth movement using a sine wave curve.
 */
public class DashAbility
{
    public float DashDuration { get; set; } = 0.8f;
    public float DashSpeed { get; set; } = 700f;
    public float TransitionDuration { get; set; } = 0.1f; // Time for start/end animations
    
    public bool IsActive { get; private set; }
    public bool IsInvincible { get; private set; }
    public Vector2 Direction { get; private set; }
    
    private float stateTimer;
    private DashState currentState = DashState.Ready;
    
    /**
     * @brief Enum representing the different states of the dash ability.
     * @details This enum defines the possible states: Ready, Starting, Dashing, Ending, and Cooldown.
     */
    public enum DashState
    {
        Ready,         // Can start dash
        Starting,      // Playing start animation
        Dashing,       // Moving with dash speed
        Ending,        // Playing end animation (reverse of start)
        Cooldown       // Optional cooldown period
    }
    
    /**
     * @brief Updates the dash ability state based on game time and movement direction.
     * @param gameTime The current game time.
     * @param movementDirection The direction of movement input.
     * @param velocity The current velocity of the character, which will be modified during the dash.
     */
    public void Update(GameTime gameTime, Vector2 movementDirection, ref Vector2 velocity)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        stateTimer -= deltaTime;
        
        switch (currentState)
        {
            case DashState.Ready:
                if (Keyboard.GetState().IsKeyDown(Keys.Space) && movementDirection != Vector2.Zero)
                {
                    StartDash(movementDirection);
                }
                break;
                
            case DashState.Starting:
                if (stateTimer <= 0)
                {
                    TransitionToState(DashState.Dashing, DashDuration);
                }
                break;
                
            case DashState.Dashing:
                ApplyDashMovement(ref velocity);
                if (stateTimer <= 0)
                {
                    TransitionToState(DashState.Ending, TransitionDuration);
                }
                break;
                
            case DashState.Ending:
                if (stateTimer <= 0)
                {
                    EndDash(ref velocity);
                }
                break;
            case DashState.Cooldown:
                if (stateTimer <= 0)
                {
                    TransitionToState(DashState.Ready, 0);
                }
                break;
        }
        
        UpdateInvincibility();
    }
    
    /**
     * @brief Starts the dash ability with the specified direction.
     * @param direction The direction in which to dash, normalized to ensure consistent speed.
     */
    private void StartDash(Vector2 direction)
    {
        IsActive = true;
        IsInvincible = true;
        Direction = Vector2.Normalize(direction);
        TransitionToState(DashState.Starting, TransitionDuration);
    }
    
    /**
     * @brief Transitions to a new dash state with the specified duration.
     * @param newState The new state to transition to.
     * @param duration The duration of the new state.
     * @details This method updates the current state and resets the state timer.
     */
    private void TransitionToState(DashState newState, float duration)
    {
        currentState = newState;
        stateTimer = duration;
        
        // Reset invincibility when starting the dash proper
        if (newState == DashState.Dashing)
        {
            IsInvincible = true;
        }
    }
    
    /**
     * @brief Applies dash movement based on the current state and direction.
     * @param velocity The current velocity of the character, which will be modified during the dash.
     * @details This method uses a sine wave to create a smooth acceleration and deceleration effect during the dash.
     */
    private void ApplyDashMovement(ref Vector2 velocity)
    {
        // Smooth curve using sine wave (peaks at midpoint)
        float progress = stateTimer / DashDuration;
        float speed = DashSpeed * (float)Math.Sin(progress * Math.PI);
        velocity = Direction * speed;
    }
    
    /**
     * @brief Ends the dash ability, resetting the state and velocity.
     * @param velocity The current velocity of the character, which will be set to zero after the dash ends.
     * @details This method transitions to the Ready state and resets the velocity.
     */
    private void EndDash(ref Vector2 velocity)
    {
        IsActive = false;
        velocity = Vector2.Zero;
        TransitionToState(DashState.Ready, 0);
    }
    
    /**
     * @brief Updates the invincibility state based on the current dash state.
     * @details This method ensures that the character remains invincible during the dash sequence.
     */
    private void UpdateInvincibility()
    {
        // Remain invincible during entire dash sequence
        IsInvincible = currentState == DashState.Starting || 
                      currentState == DashState.Dashing || 
                      currentState == DashState.Ending;
    }
    
    /**
     * @brief Update the current state of the dash ability.
     */
    public bool ShouldPlayStartAnimation() => currentState == DashState.Starting;
    
    /**
     * @brief Update the current state of the dash ability.
     */
    public bool ShouldPlayEndAnimation() => currentState == DashState.Ending;
    
    /**
     * @brief Resets the dash ability to its initial state.
     * @details This method sets the dash state to Ready, deactivates the ability, and resets the invincibility.
     */
    public void Reset()
    {
        IsActive = false;
        IsInvincible = false;
        currentState = DashState.Ready;
        stateTimer = 0;
    }
}