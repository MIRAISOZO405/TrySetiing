using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public InputActionAsset actionAsset; // Inspectorからアサインする

    private InputActionMap playerMap;
    private InputActionMap uiMap;


    void Awake()
    {
        if (actionAsset == null)
        {
            Debug.LogError("InputActionAssetがアタッチされていません。");
            return;
        }

        // ActionMapの取得
        playerMap = actionAsset.FindActionMap("Player");
        uiMap = actionAsset.FindActionMap("UI");
    }

    public void SwitchToPlayer()
    {
        // UIのActionMapを無効化
        uiMap.Disable();

        // GameplayのActionMapを有効化
        playerMap.Enable();
    }

    public void SwitchToUI()
    {
        // GameplayのActionMapを無効化
        playerMap.Disable();

        // UIのActionMapを有効化
        uiMap.Enable();
    }
}
