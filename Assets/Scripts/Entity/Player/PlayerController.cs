using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private MoveInfo curMove;
}

public static class PlayerStat
{
    public static void InitPlayerStat()
    {
        #region _Set moveDict_
        moveDict.Add("Untagged", MoveInfo.defaultInfo); // �⺻ ��
        #endregion
    }

    private static Dictionary<string, MoveInfo> moveDict = new Dictionary<string, MoveInfo>();

    public static MoveInfo GetMoveInfo(string groundTag)
    {
        MoveInfo value;
        if (!moveDict.TryGetValue(groundTag, out value)) // moveDIct���� �̵� ���� ��������
        { // �̵� ������ ���� ���
            value = MoveInfo.defaultInfo;

            Debug.LogError(groundTag + " �̵� ���� �ε� ����.");
        }

        return value;
    }
}

