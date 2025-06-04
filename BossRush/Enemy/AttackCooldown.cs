namespace BossRush.Enemy;

public class AttackCooldown
{
    public float Cooldown { get; private set; }
    public float CurrentCooldown { get; private set; }
    public bool CanAttack() => CurrentCooldown <= 0f;

    public AttackCooldown(float cooldown)
    {
        Cooldown = cooldown;
        CurrentCooldown = 0;
    }

    public void SetCooldown()
    {
        CurrentCooldown = Cooldown;
    }

    public void Update(float deltaTime)
    {
        if (CurrentCooldown > 0)
        {
            CurrentCooldown -= deltaTime;
        }
    }
}