public interface IDamageable
{
    public float MaxHP { get; protected set; }
    public float CurHP { get; protected set; }

    public void Damage(float damage);
    public void Die();
}
