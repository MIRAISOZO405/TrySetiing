using System;
using System.Collections.Generic;
using UnityEngine;
using PlayerEnums;


[Serializable]
public class LevelData
{
    public GameObject model;    // モデル
    public int rent;       // 家賃
    public int maxScore;    // 最大スコア

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
    [Header("プレイヤーLv"), SerializeField] private PlayerLevel playerLevel;
    [Header("交代エフェクト"), SerializeField] private GameObject changeEffect; // キャラ交換時に出るエフェクト(プレハブ)
    [Header("キャラプレハブ"), SerializeField] private LevelData[] levelData;
    private Transform playerTransform;

    RentalIncome rentalIncome;
    ScoreManager scoreManager;

    private void Start()
    {
       rentalIncome = FindObjectOfType<RentalIncome>();
        if (!rentalIncome)
        {
            Debug.LogError("RentalIncome.csが見つかりません");
        }
        else
        {
            playerLevel = PlayerLevel.House;
            rentalIncome.SetRent(levelData[(int)playerLevel].rent);
        }

        scoreManager = FindObjectOfType<ScoreManager>();
        if(!scoreManager)
        {
            Debug.LogError("ScoreManager.csが見つかりません");
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
        instantiatedPrefab.transform.localScale = scale / 2/*new Vector3(2.0f, 2.0f, 2.0f)*/; // 例: 元の2倍の大きさにする

        // プレイヤーレベルとスコアに対応する数字を取得
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
        // 現在のプレイヤーレベルに対応する LevelData を取得
        LevelData currentLevelData = levelData[(int)playerLevel];

        // 引数の score に応じて適切なスコアを取得
        switch (score)
        {
            case ScoreEnum.Easy:
                return currentLevelData.difficultyScore.Easy;
            case ScoreEnum.Normal:
                return currentLevelData.difficultyScore.Normal;
            case ScoreEnum.Hard:
                return currentLevelData.difficultyScore.Hard;
            default:
                return 0; // 不明なスコアの場合、0 を返すなど適切な処理を行ってください
        }
    }
}