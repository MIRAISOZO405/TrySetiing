using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public InputActionAsset actionAsset; // Inspector����A�T�C������

    private InputActionMap playerMap;
    private InputActionMap uiMap;


    void Awake()
    {
        if (actionAsset == null)
        {
            Debug.LogError("InputActionAsset���A�^�b�`����Ă��܂���B");
            return;
        }

        // ActionMap�̎擾
        playerMap = actionAsset.FindActionMap("Player");
        uiMap = actionAsset.FindActionMap("UI");
    }

    public void SwitchToPlayer()
    {
        // UI��ActionMap�𖳌���
        uiMap.Disable();

        // Gameplay��ActionMap��L����
        playerMap.Enable();
    }

    public void SwitchToUI()
    {
        // Gameplay��ActionMap�𖳌���
        playerMap.Disable();

        // UI��ActionMap��L����
        uiMap.Enable();
    }
}
