using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MoneyManager : MonoBehaviour
{
    [Header("所持金"), SerializeField] private int money;
    [Header("アニメーション持続時間"), SerializeField] private float durationTime = 1f;
    private Image image; // お金のImage画像(pivot.xを1にしておく)
    private Text text;  
    private MoneyReflection shopDisplay;    // shop用の金表示
    private int targetMoney;                // 目標金額を保持するための変数

    private void Start()
    {
        money = 0;
        text = gameObject.GetComponent<Text>();
        image = GetComponentInChildren<Image>(); // 自身の子からImageコンポーネントを取得

        shopDisplay = FindObjectOfType<MoneyReflection>();
        if(!shopDisplay)
        {
            Debug.LogError("MoneyReflectionコンポーネントが見つかりません");
        }

        UpdateMoneyDisplay();
    }

    public void AddMoney(int amount)
    {
        targetMoney += amount;

        if (targetMoney < 0)
            targetMoney = 0;

        DOTween.To(() => money, x => money = x, targetMoney, durationTime) // 1.0fはアニメーション時間です。必要に応じて変更してください
           .OnUpdate(() =>
           {
               text.text = "" + FormatMoney(money);
               AdjustTextWidth();
               shopDisplay.CopyText(text.text);
           });
    }

    private void Update()
    {
        // あとで消す（モデルチェンジ）
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AddMoney(10000000);
        }
    }

    // 3桁区切り
    private string FormatMoney(int amount)
    {
        return string.Format("{0:N0}", amount);
    }

    private void UpdateMoneyDisplay()
    {
        text.text = FormatMoney(money);
        AdjustTextWidth();

        if (shopDisplay)
        {
            shopDisplay.CopyText(text.text);
        }
    }

    // Textのwidth幅を、文字数に応じて変更
    private void AdjustTextWidth()
    {
        // テキストの推奨幅を取得
        float newWidth = text.preferredWidth;

        // RectTransformの幅を更新
        text.rectTransform.sizeDelta = new Vector2(newWidth, text.rectTransform.sizeDelta.y);

        // imageの位置も変更する
        image.rectTransform.anchoredPosition = new Vector2( - newWidth, image.rectTransform.anchoredPosition.y);
    }

    public int GetMoney()
    {
        return money;
    }

}
