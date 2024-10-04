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
        moveDict.Add("Untagged", MoveInfo.defaultInfo); // 기본 땅
        #endregion
    }

    private static Dictionary<string, MoveInfo> moveDict = new Dictionary<string, MoveInfo>();

    public static MoveInfo GetMoveInfo(string groundTag)
    {
        MoveInfo value;
        if (!moveDict.TryGetValue(groundTag, out value)) // moveDIct에서 이동 정보 가져오기
        { // 이동 정보가 없을 경우
            value = MoveInfo.defaultInfo;

            Debug.LogError(groundTag + " 이동 정보 로드 실패.");
        }

        return value;
    }
}

