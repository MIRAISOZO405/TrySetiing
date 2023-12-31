using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RentalIncome : MonoBehaviour
{
    private ScoreManager scoreManager;    // 家賃収入用
    private MoneyManager moneyManager;    // 家賃収入用
    private LevelManager levelManager;

    private int rent;   // 家賃

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        moneyManager = FindObjectOfType<MoneyManager>();
        levelManager = FindObjectOfType<LevelManager>();

        if (!scoreManager)
        {
            Debug.LogError("ScoreManager.csが見つかりません");
        }
        if (!moneyManager)
        {
            Debug.LogError("MoneyManager.csが見つかりません");
        }
        if (!levelManager)
        {
            Debug.LogError("LevelManager.csが見つかりません");
        }
    }
   
    public void RentPayment()
    {
        int score = scoreManager.GetScore();
        int payment = score * rent;

        moneyManager.AddMoney(payment);
    }

    public void SetRent(int newRent)
    {
        rent = newRent;
    }
}
