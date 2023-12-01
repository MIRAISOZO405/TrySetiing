using UnityEngine;
using PlayerEnums;

public class ShopManager : MonoBehaviour
{
    private PlayerLevel goodsSelect;
    private bool isTryal = false; // ��������


    private GameObject shopDisplay;

    private GameObject apart;
    private GameObject mansion;
    private GameObject towerMansion;
    private GameObject currentSelect;

    private Animation goodsAnim;
    private GameObject goodsMark;

    private RenderChange renderChange;

    private void Start()
    {
        shopDisplay = transform.Find("ShopDisplay").gameObject;

        if (!shopDisplay)
        {
            Debug.LogError("ShopDisplay��������܂���");
        }

        goodsMark = GameObject.Find("GoodsMark").gameObject;

        if (goodsMark)
        {
            goodsAnim = goodsMark.GetComponent<Animation>();
        }
        else
        {
            Debug.LogError("GoodsMark��������܂���");
        }

        apart = shopDisplay.transform.Find("Apart").gameObject;
        mansion = shopDisplay.transform.Find("Mansion").gameObject;
        towerMansion = shopDisplay.transform.Find("TowerMansion").gameObject;
        if (!apart || !mansion || !towerMansion)
        {
            Debug.LogError("���i��������܂���");
        }

        renderChange = GameObject.Find("RenderCamera_Player").GetComponent<RenderChange>();
        if (!renderChange)
        {
            Debug.LogError("renderChange�R���|�[�l���g��������܂���");
        }
    }

    // �J�����я����ʒu��
    public void GoodsReset()
    {
        // �V�[�����̂��ׂĂ�GoodsPrice�R���|�[�l���g���擾
        GoodsPrice[] allGoodsPrices = FindObjectsOfType<GoodsPrice>();

        // �eGoodsPrice�R���|�[�l���g��SetSelect�֐����Ăяo��
        foreach (GoodsPrice goodsAnim in allGoodsPrices)
        {
            goodsAnim.SetSelect(false);
        }

        goodsSelect = PlayerLevel.Apart;
        Change();
        isTryal = false; // ��������


    }

public void GoodsChange(Vector2 direction)
    {
        if (direction.x > 0.5f)
        {
            if (goodsSelect == PlayerLevel.TowerMansion)
                goodsSelect = PlayerLevel.Apart;
            else
                goodsSelect += 1;
        }
        else if (direction.x < -0.5f)
        {
            if (goodsSelect == PlayerLevel.Apart)
                goodsSelect = PlayerLevel.TowerMansion;
            else
                goodsSelect -= 1;
        }

        Change();
    }

    private void Change()
    {
        switch (goodsSelect)
        {
            case PlayerLevel.Apart:
                currentSelect = apart;
                break;
            case PlayerLevel.Mansion:
                currentSelect = mansion;
                break;
            case PlayerLevel.TowerMansion:
                currentSelect = towerMansion;
                break;
           default:
                Debug.LogError("�\�����ʃG���[");
                break;
        }

        // �V�[�����̂��ׂĂ�GoodsPrice�R���|�[�l���g���擾
        GoodsPrice[] allGoodsPrices = FindObjectsOfType<GoodsPrice>();

        // �eGoodsPrice�R���|�[�l���g��SetSelect�֐����Ăяo��
        foreach (GoodsPrice goodsAnim in allGoodsPrices)
        {
            goodsAnim.SetSelect(false);
        }

        currentSelect.GetComponent<GoodsPrice>().SetSelect(true);
        goodsMark.transform.position = currentSelect.transform.position;

        if (!isTryal)
        {
            // ���ʂ̃��f���`��
            renderChange.CurrentModelChange();
            return;
        }

        renderChange.ModelChange(goodsSelect);

    }

    public void GoodsConfirm()
    {
        if (!currentSelect.GetComponent<GoodsPrice>().ChackBuy())
        {
            goodsAnim.Play();
            currentSelect.GetComponent<GoodsPrice>().AnimPlay();
        }
        else
        {
            Debug.Log("�w�����s�̏���");
        }

    }

    public void GoodsTrial()
    {
        isTryal = !isTryal;

        if (!isTryal)
        {
            // ���ʂ̃��f���`��
            renderChange.CurrentModelChange();
            return;
        }

        renderChange.ModelChange(goodsSelect);

    }
}
