using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RentalIncome : MonoBehaviour
{
    private ScoreManager scoreManager;    // ‰Æ’ÀŽû“ü—p
    private MoneyManager moneyManager;    // ‰Æ’ÀŽû“ü—p
    private LevelManager levelManager;

    private int rent;   // ‰Æ’À

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        moneyManager = FindObjectOfType<MoneyManager>();
        levelManager = FindObjectOfType<LevelManager>();

        if (!scoreManager)
        {
            Debug.LogError("ScoreManager.cs‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñ");
        }
        if (!moneyManager)
        {
            Debug.LogError("MoneyManager.cs‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñ");
        }
        if (!levelManager)
        {
            Debug.LogError("LevelManager.cs‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñ");
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
