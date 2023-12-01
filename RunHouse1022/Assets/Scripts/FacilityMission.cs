using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerEnums;

public class FacilityMission : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";
    private const string MISSION_DISPLAY_NAME = "MissionDisplay";

    [Header("ミッションプレハブ"), SerializeField]
    public GameObject missionPrefab;

    [Header("ミッション発生中"), SerializeField]
    public bool missionFlg = false;

    [Header("ミッション番号"), SerializeField]
    public int missionNo = 0;

    [Header("難易度"), SerializeField]
    private ScoreEnum scoreEnum;
    //[Header("クリア時にスコア加算されるポイント"), SerializeField]
    //public int point = 10;

    private MissionDisplay missionDisplay;
    private LevelManager levelManager;

    private void Start()
    {
        missionDisplay = FindObjectOfType<MissionDisplay>();
        levelManager = FindObjectOfType<LevelManager>();

        if (!missionDisplay)
        {
            Debug.LogError("MissionDisplay.csが見つかりません");
        }
        if (!levelManager)
        {
            Debug.LogError("LevelManager.csが見つかりません");
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!missionFlg || !col.CompareTag(PLAYER_TAG))
            return;

        missionFlg = false;

        if (missionDisplay)
        {
            int point = levelManager.GetScore(scoreEnum);
            Debug.Log("ポイント" + point);
            missionDisplay.ClearMission(missionNo,point);  // ClearMissionメソッドを呼び出す
        }

        missionNo = 0;
    }
}
