using UnityEngine;
using UnityEngine.UI;
using PlayerEnums;

public class GoodsPrice : MonoBehaviour
{
    [Header("販売しているLv"), SerializeField]
    private PlayerLevel saleLevel;

    [Header("価格"), SerializeField]
    private int price;

    [Header("選択中かどうか"), SerializeField]
    private bool isSelect = false;

    [Header("販売中かどうか"), SerializeField]
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

        if (!text) Debug.LogError("Textが見つかりません");

        levelManager = FindObjectOfType<LevelManager>();
        if (!levelManager) Debug.LogError("levelManagerが見つかりません");

        moneyManager = FindObjectOfType<MoneyManager>();
        if (!moneyManager) Debug.LogError("MoneyManagerが見つかりません");

        text.text = "￥" + FormatMoney(price);
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
            if (!uiController) Debug.LogError("Playerが見つかりません");
        }

        uiController.OnQuit();
        moneyManager.AddMoney(-price);
        levelManager.SetLevel(saleLevel);
    }
}
