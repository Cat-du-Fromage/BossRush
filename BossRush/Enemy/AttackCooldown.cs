namespace BossRush.Enemy;

/**
 * Handles attack cooldown timers for game entities.
 * Manages time intervals between consecutive attacks.
 */
public class AttackCooldown
{
    /**
     * The total cooldown duration in seconds
     */
    public float Cooldown { get; private set; }

    /**
     * Remaining time before next available attack
     */
    public float CurrentCooldown { get; private set; }

    /**
     * Checks if an attack can be performed
     * @return True if cooldown has expired, false otherwise
     */
    public bool CanAttack() => CurrentCooldown <= 0f;

    /**
     * Initializes a new cooldown timer
     * @param cooldown Total cooldown duration in seconds
     */
    public AttackCooldown(float cooldown)
    {
        Cooldown = cooldown;
        CurrentCooldown = -1; // Ready to attack immediately
    }

    /**
     * Activates the cooldown period after an attack
     * Resets the timer to full duration
     */
    public void SetCooldown()
    {
        CurrentCooldown = Cooldown;
    }

    /**
     * Updates the cooldown timer
     * @param deltaTime Time elapsed since last frame (in seconds)
     */
    public void Update(float deltaTime)
    {
        if (CurrentCooldown > 0)
        {
            CurrentCooldown -= deltaTime;
        }
    }
}