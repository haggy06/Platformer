using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class SettingData
{
    #region _Key Setting_
    private static KeyData_Editable _keyData = new KeyData_Editable();
    public static KeyData keyData => _keyData;
    public static KeyData_Editable keyData_Edit => _keyData;
    public static void ResetKey()
    {
        _keyData = new KeyData_Editable();
    }
    #endregion
}

public class KeyData
{
    public static readonly KeyCode interact = KeyCode.Z;

    protected KeyCode _leftMove = KeyCode.LeftArrow;
    protected KeyCode _rightMove = KeyCode.RightArrow;
    protected KeyCode _upMove = KeyCode.UpArrow;
    protected KeyCode _downMove = KeyCode.DownArrow;

    protected KeyCode _jump = KeyCode.Z;
    protected KeyCode _dash = KeyCode.C;

    protected KeyCode _attack = KeyCode.X;

    protected KeyCode _shoot = KeyCode.A;
    protected KeyCode _swap = KeyCode.LeftShift;

    protected KeyCode _map = KeyCode.Tab;
    protected KeyCode _inventory = KeyCode.Space;


    public KeyCode leftMove => _leftMove;
    public KeyCode rightMove => _rightMove;
    public KeyCode upMove => _upMove;
    public KeyCode downMove => _downMove;


    public KeyCode jump => _jump;
    public KeyCode dash => _dash;
    public KeyCode attack => _attack;

    public KeyCode shoot => _shoot;
    public KeyCode swap => _swap;

    public KeyCode map => _map;
    public KeyCode inventory => _inventory;
}
public class KeyData_Editable : KeyData
{
    public new KeyCode leftMove { get => _leftMove; set => _leftMove = value; }
    public new KeyCode rightMove { get => _rightMove; set => _rightMove = value; }
    public new KeyCode upMove { get => _upMove; set => _upMove = value; }
    public new KeyCode downMove { get => _downMove; set => _downMove = value; }


    public new KeyCode jump { get => _jump; set => _jump = value; }
    public new KeyCode dash { get => _dash; set => _dash = value; }

    public new KeyCode attack { get => _attack; set => _attack = value; }

    public new KeyCode shoot { get => _shoot; set => _shoot = value; }
    public new KeyCode swap { get => _swap; set => _swap = value; }

    public new KeyCode map { get => _map; set => _map = value; }
    public new KeyCode inventory { get => _inventory; set => _inventory = value; }
}