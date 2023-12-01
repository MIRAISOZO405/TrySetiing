using UnityEngine;
using UnityEngine.UI;
using PlayerEnums;

public class GoodsPrice : MonoBehaviour
{
    [Header("�̔����Ă���Lv"), SerializeField]
    private PlayerLevel saleLevel;

    [Header("���i"), SerializeField]
    private int price;

    [Header("�I�𒆂��ǂ���"), SerializeField]
    private bool isSelect = false;

    [Header("�̔������ǂ���"), SerializeField]
    private bool soldOut = false;

    private LevelManager levelManager;
    private MoneyManager moneyManager;
    private UIController uiController;
    private Animation anim;
    private Text text;

    private void Start()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        anim = GetComponent<Animation>();
        text = transform.Find("PriceText").GetComponent<Text>();

        if (!text) Debug.LogError("Text��������܂���");

        levelManager = FindObjectOfType<LevelManager>();
        if (!levelManager) Debug.LogError("levelManager��������܂���");

        moneyManager = FindObjectOfType<MoneyManager>();
        if (!moneyManager) Debug.LogError("MoneyManager��������܂���");

        text.text = "��" + FormatMoney(price);
        AdjustTextWidth();
    }

    public void SetSelect(bool select)
    {
        isSelect = select;
    }

    public void AnimPlay()
    {
        anim.Play();
        Buy();
    }

    private string FormatMoney(int amount)
    {
        return amount.ToString("N0").Replace(",", ".");
    }

    private void AdjustTextWidth()
    {
        float newWidth = text.preferredWidth;
        text.rectTransform.sizeDelta = new Vector2(newWidth, text.rectTransform.sizeDelta.y);
    }

    public bool ChackBuy()
    {
        if (moneyManager.GetMoney() < price)
            return true;

        return soldOut;
    }

    public void Buy()
    {

        soldOut = true;

        uiController = GameObject.FindGameObjectWithTag("Player").GetComponent<UIController>();
        if (!uiController)
        {     
            if (!uiController) Debug.LogError("Player��������܂���");
        }

        uiController.OnQuit();
        moneyManager.AddMoney(-price);
        levelManager.SetLevel(saleLevel);
    }
}
