using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(GravityEntity))]
public class PlayerController : Singleton<PlayerController>
{
    public bool controllable = true;

    private GravityEntity control;
    protected override void Awake()
    {
        base.Awake();
        control = GetComponent<GravityEntity>();
    }

    protected override void SceneChanged(Scene replacedScene, Scene newScene)
    {

    }

    [SerializeField]
    private int moveDir;
    private void Update()
    {
        if (controllable)
        {
            moveDir = 0;
            if (Input.GetKey(SettingData.keyData.leftMove))
                moveDir--;
            if (Input.GetKey(SettingData.keyData.rightMove))
                moveDir++;

            if (Input.GetKeyDown(SettingData.keyData.jump))
            {
                control.Jump();
            }
            else if (Input.GetKeyUp(SettingData.keyData.jump))
            {
                control.JumpCancle();
            }

            if (Input.GetKeyDown(SettingData.keyData.dash))
                GetComponent<Rigidbody2D>().AddForce(Vector2.right * (control.LookDir ? 1f : -1f) * 5f, ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate()
    {
        control.Move(moveDir, 0);
    }
}