using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class UIRotation : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera; // ƒJƒƒ‰‚ğæ“¾

    void Start()
    {
        virtualCamera = GameObject.Find("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        if (!virtualCamera)
        {
            Debug.LogError("virtualCamera‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñ");
        }
    }

    private void LateUpdate()
    {
      //@ƒJƒƒ‰‚Æ“¯‚¶Œü‚«‚Éİ’è
      transform.rotation = virtualCamera.transform.rotation;
    }
}
