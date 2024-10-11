using UnityEngine;

public interface IDamageable
{
    public EntityType EntityType { get; }

    public int MaxHP { get; }
    public int CurHP { get; }

    public void Damage(Vector2 hitDirection, int damage);
    public void Die(Vector2 deadDirection);
}

public enum EntityType
{
    Player,
    Monster,
    Object
}