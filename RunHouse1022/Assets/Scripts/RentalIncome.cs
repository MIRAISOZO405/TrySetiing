using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RentalIncome : MonoBehaviour
{
    private ScoreManager scoreManager;    // �ƒ������p
    private MoneyManager moneyManager;    // �ƒ������p
    private LevelManager levelManager;

    private int rent;   // �ƒ�

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        moneyManager = FindObjectOfType<MoneyManager>();
        levelManager = FindObjectOfType<LevelManager>();

        if (!scoreManager)
        {
            Debug.LogError("ScoreManager.cs��������܂���");
        }
        if (!moneyManager)
        {
            Debug.LogError("MoneyManager.cs��������܂���");
        }
        if (!levelManager)
        {
            Debug.LogError("LevelManager.cs��������܂���");
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
