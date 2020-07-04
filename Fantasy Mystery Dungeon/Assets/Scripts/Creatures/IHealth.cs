public interface IHealth
{
    int MaxHealth { get; }
    int Health { get; }
    void TakeDamage(int amount, DamageTypes damType);
    void Heal(int amount);
    void Die();
}