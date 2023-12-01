using System;
using System.Collections.Generic;
using UnityEngine;
using PlayerEnums;


[Serializable]
public class LevelData
{
    public GameObject model;    // ���f��
    public int rent;       // �ƒ�
    public int maxScore;    // �ő�X�R�A

    [Serializable]
    public class DifficultyScore
    {
        public int Easy;
        public int Normal;
        public int Hard;
    }

    public DifficultyScore difficultyScore = new DifficultyScore();
}

public class LevelManager : MonoBehaviour
{
    [Header("�v���C���[Lv"), SerializeField] private PlayerLevel playerLevel;
    [Header("���G�t�F�N�g"), SerializeField] private GameObject changeEffect; // �L�����������ɏo��G�t�F�N�g(�v���n�u)
    [Header("�L�����v���n�u"), SerializeField] private LevelData[] levelData;
    private Transform playerTransform;

    RentalIncome rentalIncome;
    ScoreManager scoreManager;

    private void Start()
    {
       rentalIncome = FindObjectOfType<RentalIncome>();
        if (!rentalIncome)
        {
            Debug.LogError("RentalIncome.cs��������܂���");
        }
        else
        {
            playerLevel = PlayerLevel.House;
            rentalIncome.SetRent(levelData[(int)playerLevel].rent);
        }

        scoreManager = FindObjectOfType<ScoreManager>();
        if(!scoreManager)
        {
            Debug.LogError("ScoreManager.cs��������܂���");
        }
    }

    public void SetLevel(PlayerLevel lv)
    {
        playerLevel = lv;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 playerPosition = playerTransform.position;
        Quaternion playerRotation = Quaternion.Euler(0, playerTransform.rotation.eulerAngles.y, 0);
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Vector3 scale = Vector3.one;

        Instantiate(levelData[(int)playerLevel].model, playerPosition, playerRotation);
        rentalIncome.SetRent(levelData[(int)playerLevel].rent);
        scoreManager.SetMaxScore(levelData[(int)playerLevel].maxScore);

        GameObject instantiatedPrefab = Instantiate(changeEffect, playerPosition, Quaternion.identity) as GameObject;
        instantiatedPrefab.transform.localScale = scale / 2/*new Vector3(2.0f, 2.0f, 2.0f)*/; // ��: ����2�{�̑傫���ɂ���

        // �v���C���[���x���ƃX�R�A�ɑΉ����鐔�����擾
        LevelData.DifficultyScore stats = levelData[(int)playerLevel].difficultyScore;
        int easyValue = stats.Easy;
        int normalValue = stats.Normal;
        int hardValue = stats.Hard;

        Debug.Log(easyValue);
    }

    public PlayerLevel GetLevel()
    {
        return playerLevel;
    }

    public int GetScore(ScoreEnum score)
    {
        // ���݂̃v���C���[���x���ɑΉ����� LevelData ���擾
        LevelData currentLevelData = levelData[(int)playerLevel];

        // ������ score �ɉ����ēK�؂ȃX�R�A���擾
        switch (score)
        {
            case ScoreEnum.Easy:
                return currentLevelData.difficultyScore.Easy;
            case ScoreEnum.Normal:
                return currentLevelData.difficultyScore.Normal;
            case ScoreEnum.Hard:
                return currentLevelData.difficultyScore.Hard;
            default:
                return 0; // �s���ȃX�R�A�̏ꍇ�A0 ��Ԃ��ȂǓK�؂ȏ������s���Ă�������
        }
    }
}