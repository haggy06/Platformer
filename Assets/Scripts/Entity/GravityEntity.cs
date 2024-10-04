using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class GravityEntity : MonoBehaviour
{
    [Header("Move Setting")]
    [SerializeField]
    protected float moveSpeed = 5f;
    [SerializeField]
    protected float jumpPower = 5f;

    [Header("Move Status")]
    [SerializeField]
    protected bool isGround = false;
    [SerializeField]
    protected GroundData groundData;

    protected BoxCollider2D col;
    protected Rigidbody2D rigid2D;
    protected void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        rigid2D = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        Vector2 footPosition = new Vector2(col.bounds.center.x, col.bounds.min.y);
        Vector2 offset = new Vector2((col.bounds.size.x / 2f) - 0.01f, 0f);
        Collider2D contactedGround = Physics2D.OverlapArea(footPosition - offset, footPosition + offset, 1 << (int)LAYER.Ground);

        isGround = contactedGround;

        if (contactedGround) // ������ ���� ���� ���
            if (!contactedGround.TryGetComponent<GroundData>(out groundData)) // ���� GroundData ����
                groundData = contactedGround.AddComponent<GroundData>(); // GroundData�� ������ ��� ������Ʈ �߰� �� ����
    }
}

public enum LAYER
{
    Default,
    TtansParentFX,
    Ignore_Raycast,
    Ground,
    Water,
    UI,

}