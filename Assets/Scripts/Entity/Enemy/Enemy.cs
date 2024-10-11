using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(EnemyStateMachine))]
public abstract class Enemy : MonoBehaviour, IDamageable, IMoveable
{
    [SerializeField]
    protected EntityType entityType;
    public EntityType EntityType => entityType;

    #region _IDamageable_
    [Header("Damage Setting")]
    [SerializeField]
    protected int maxHP = 10;
    public int MaxHP => maxHP;
    [SerializeField]
    protected int curHP = 10;
    public int CurHP => curHP;

    [Space(5)]
    [SerializeField]
    protected bool useKnockback = true;
    [SerializeField]
    protected float damageKnockback = 5f;

    [Space(5)]
    [SerializeField]
    protected float deadKnockback = 15f;
    [SerializeField]
    protected GameObject deadBody;

    public virtual void Damage(Vector2 hitDirection, int damage)
    {
        curHP -= damage;
        if (useKnockback)
            rigid2D.velocity = hitDirection * damageKnockback;

        if (curHP <= 0)
            Die(hitDirection);
    }

    public virtual void Die(Vector2 deadDirection)
    {
        if (deadBody != null)
        {
            GameObject myDeadBody = Instantiate(deadBody, transform); // Todo : 오브젝트 풀링 구현 후 풀링 쓰도록 수정하기
            if (useKnockback)
                myDeadBody.GetComponent<Rigidbody2D>().velocity = deadDirection * deadKnockback;
        }
    }
    #endregion

    #region _IMoveable_
    [field: Header("Move Setting")]
    [field: SerializeField]
    public float MoveSpeed { get; set; } = 5f;
    [field: SerializeField]
    public float JumpPower { get; set; } = 10f;
    [SerializeField]
    protected MoveData moveData;
    public MoveData MoveData => moveData;

    public abstract void Move(Vector2 move);

    public abstract void Stop();

    public abstract void Turn(bool rightDirection);

    public abstract void Jump();
    #endregion

    protected Rigidbody2D rigid2D;
    protected BoxCollider2D hitBox;
    protected EnemyStateMachine enemyState;
    protected virtual void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        hitBox = GetComponent<BoxCollider2D>();
        enemyState = GetComponent<EnemyStateMachine>();
    }
}
