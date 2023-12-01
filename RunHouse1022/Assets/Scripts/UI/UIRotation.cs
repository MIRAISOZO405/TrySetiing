using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class UIRotation : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera; // �J�������擾

    void Start()
    {
        virtualCamera = GameObject.Find("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        if (!virtualCamera)
        {
            Debug.LogError("virtualCamera��������܂���");
        }
    }

    private void LateUpdate()
    {
      //�@�J�����Ɠ��������ɐݒ�
      transform.rotation = virtualCamera.transform.rotation;
    }
}
