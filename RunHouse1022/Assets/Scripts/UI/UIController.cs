using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    public enum ActiveUI
    {
        None,
        Shop,
        //Rsesult
    }
    public ActiveUI currentActiveUI = ActiveUI.None;

    private ShopManager shopManager;

    private void Start()
    {
        shopManager = GameObject.Find("ShopCanvas").GetComponent<ShopManager>();
        if (!shopManager)
        {
            Debug.LogError("shopCanvas��������܂���");
        }
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();

        shopManager.GoodsChange(direction);     
    }

    public void OnConfirm()
    {
        shopManager.GoodsConfirm();
    }

    public void OnQuit()
    {
        // shopCanvas�݂̂��\��
        GetComponent<PlayerController>().OnReturnMap();
    }

    public void OnTrial()
    {
        shopManager.GoodsTrial();
    }
}
