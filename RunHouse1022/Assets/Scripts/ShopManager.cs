using UnityEngine;
using PlayerEnums;

public class ShopManager : MonoBehaviour
{
    private PlayerLevel goodsSelect;
    private bool isTryal = false; // お試し中


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
            Debug.LogError("ShopDisplayが見つかりません");
        }

        goodsMark = GameObject.Find("GoodsMark").gameObject;

        if (goodsMark)
        {
            goodsAnim = goodsMark.GetComponent<Animation>();
        }
        else
        {
            Debug.LogError("GoodsMarkが見つかりません");
        }

        apart = shopDisplay.transform.Find("Apart").gameObject;
        mansion = shopDisplay.transform.Find("Mansion").gameObject;
        towerMansion = shopDisplay.transform.Find("TowerMansion").gameObject;
        if (!apart || !mansion || !towerMansion)
        {
            Debug.LogError("商品が見つかりません");
        }

        renderChange = GameObject.Find("RenderCamera_Player").GetComponent<RenderChange>();
        if (!renderChange)
        {
            Debug.LogError("renderChangeコンポーネントが見つかりません");
        }
    }

    // 開くたび初期位置に
    public void GoodsReset()
    {
        // シーン内のすべてのGoodsPriceコンポーネントを取得
        GoodsPrice[] allGoodsPrices = FindObjectsOfType<GoodsPrice>();

        // 各GoodsPriceコンポーネントでSetSelect関数を呼び出す
        foreach (GoodsPrice goodsAnim in allGoodsPrices)
        {
            goodsAnim.SetSelect(false);
        }

        goodsSelect = PlayerLevel.Apart;
        Change();
        isTryal = false; // お試し中


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
                Debug.LogError("予期せぬエラー");
                break;
        }

        // シーン内のすべてのGoodsPriceコンポーネントを取得
        GoodsPrice[] allGoodsPrices = FindObjectsOfType<GoodsPrice>();

        // 各GoodsPriceコンポーネントでSetSelect関数を呼び出す
        foreach (GoodsPrice goodsAnim in allGoodsPrices)
        {
            goodsAnim.SetSelect(false);
        }

        currentSelect.GetComponent<GoodsPrice>().SetSelect(true);
        goodsMark.transform.position = currentSelect.transform.position;

        if (!isTryal)
        {
            // 普通のモデル描画
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
            Debug.Log("購入失敗の処理");
        }

    }

    public void GoodsTrial()
    {
        isTryal = !isTryal;

        if (!isTryal)
        {
            // 普通のモデル描画
            renderChange.CurrentModelChange();
            return;
        }

        renderChange.ModelChange(goodsSelect);

    }
}
