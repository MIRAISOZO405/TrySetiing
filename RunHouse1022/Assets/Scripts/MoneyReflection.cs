using UnityEngine;
using UnityEngine.UI;

public class MoneyReflection : MonoBehaviour
{
    private Image image; // お金のImage画像(pivot.xを1にしておく)
    private Text text;

    private void Start()
    {
        text = gameObject.GetComponent<Text>();
        image = GetComponentInChildren<Image>(); // 自身の子からImageコンポーネントを取得
    }

    public void CopyText(string value)
    {
        text.text = value;
        AdjustTextWidth();
    }

    // Textのwidth幅を、文字数に応じて変更
    private void AdjustTextWidth()
    {
        // テキストの推奨幅を取得
        float newWidth = text.preferredWidth;

        // RectTransformの幅を更新
        text.rectTransform.sizeDelta = new Vector2(newWidth, text.rectTransform.sizeDelta.y);

        // imageの位置も変更する
        image.rectTransform.anchoredPosition = new Vector2(-newWidth, image.rectTransform.anchoredPosition.y);
    }
}
