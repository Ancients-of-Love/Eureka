public interface IDamageable
{
    public float CurrentHealth { get; }

    public void Damage(float damageAmount);

    public void Heal(float healAmount);

    public void Die();
}