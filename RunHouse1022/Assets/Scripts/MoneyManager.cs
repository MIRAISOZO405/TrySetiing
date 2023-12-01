using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MoneyManager : MonoBehaviour
{
    [Header("������"), SerializeField] private int money;
    [Header("�A�j���[�V������������"), SerializeField] private float durationTime = 1f;
    private Image image; // ������Image�摜(pivot.x��1�ɂ��Ă���)
    private Text text;  
    private MoneyReflection shopDisplay;    // shop�p�̋��\��
    private int targetMoney;                // �ڕW���z��ێ����邽�߂̕ϐ�

    private void Start()
    {
        money = 0;
        text = gameObject.GetComponent<Text>();
        image = GetComponentInChildren<Image>(); // ���g�̎q����Image�R���|�[�l���g���擾

        shopDisplay = FindObjectOfType<MoneyReflection>();
        if(!shopDisplay)
        {
            Debug.LogError("MoneyReflection�R���|�[�l���g��������܂���");
        }

        UpdateMoneyDisplay();
    }

    public void AddMoney(int amount)
    {
        targetMoney += amount;

        if (targetMoney < 0)
            targetMoney = 0;

        DOTween.To(() => money, x => money = x, targetMoney, durationTime) // 1.0f�̓A�j���[�V�������Ԃł��B�K�v�ɉ����ĕύX���Ă�������
           .OnUpdate(() =>
           {
               text.text = "" + FormatMoney(money);
               AdjustTextWidth();
               shopDisplay.CopyText(text.text);
           });
    }

    private void Update()
    {
        // ���Ƃŏ����i���f���`�F���W�j
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AddMoney(10000000);
        }
    }

    // 3����؂�
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

    // Text��width�����A�������ɉ����ĕύX
    private void AdjustTextWidth()
    {
        // �e�L�X�g�̐��������擾
        float newWidth = text.preferredWidth;

        // RectTransform�̕����X�V
        text.rectTransform.sizeDelta = new Vector2(newWidth, text.rectTransform.sizeDelta.y);

        // image�̈ʒu���ύX����
        image.rectTransform.anchoredPosition = new Vector2( - newWidth, image.rectTransform.anchoredPosition.y);
    }

    public int GetMoney()
    {
        return money;
    }

}
