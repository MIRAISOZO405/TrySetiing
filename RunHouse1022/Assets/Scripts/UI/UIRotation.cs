using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class UIRotation : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera; // カメラを取得

    void Start()
    {
        virtualCamera = GameObject.Find("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        if (!virtualCamera)
        {
            Debug.LogError("virtualCameraが見つかりません");
        }
    }

    private void LateUpdate()
    {
      //　カメラと同じ向きに設定
      transform.rotation = virtualCamera.transform.rotation;
    }
}
