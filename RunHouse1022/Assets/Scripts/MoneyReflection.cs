using UnityEngine;
using UnityEngine.UI;

public class MoneyReflection : MonoBehaviour
{
    private Image image; // ������Image�摜(pivot.x��1�ɂ��Ă���)
    private Text text;

    private void Start()
    {
        text = gameObject.GetComponent<Text>();
        image = GetComponentInChildren<Image>(); // ���g�̎q����Image�R���|�[�l���g���擾
    }

    public void CopyText(string value)
    {
        text.text = value;
        AdjustTextWidth();
    }

    // Text��width�����A�������ɉ����ĕύX
    private void AdjustTextWidth()
    {
        // �e�L�X�g�̐��������擾
        float newWidth = text.preferredWidth;

        // RectTransform�̕����X�V
        text.rectTransform.sizeDelta = new Vector2(newWidth, text.rectTransform.sizeDelta.y);

        // image�̈ʒu���ύX����
        image.rectTransform.anchoredPosition = new Vector2(-newWidth, image.rectTransform.anchoredPosition.y);
    }
}
