using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerEnums;

public class FacilityMission : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";
    private const string MISSION_DISPLAY_NAME = "MissionDisplay";

    [Header("�~�b�V�����v���n�u"), SerializeField]
    public GameObject missionPrefab;

    [Header("�~�b�V����������"), SerializeField]
    public bool missionFlg = false;

    [Header("�~�b�V�����ԍ�"), SerializeField]
    public int missionNo = 0;

    [Header("��Փx"), SerializeField]
    private ScoreEnum scoreEnum;
    //[Header("�N���A���ɃX�R�A���Z�����|�C���g"), SerializeField]
    //public int point = 10;

    private MissionDisplay missionDisplay;
    private LevelManager levelManager;

    private void Start()
    {
        missionDisplay = FindObjectOfType<MissionDisplay>();
        levelManager = FindObjectOfType<LevelManager>();

        if (!missionDisplay)
        {
            Debug.LogError("MissionDisplay.cs��������܂���");
        }
        if (!levelManager)
        {
            Debug.LogError("LevelManager.cs��������܂���");
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
            Debug.Log("�|�C���g" + point);
            missionDisplay.ClearMission(missionNo,point);  // ClearMission���\�b�h���Ăяo��
        }

        missionNo = 0;
    }
}
