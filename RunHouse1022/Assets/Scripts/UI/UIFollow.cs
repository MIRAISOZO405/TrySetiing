using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollow : MonoBehaviour
{
    private GameObject player;
    private Transform playerTrans;
    private Camera mainCamera;
    [Header("�ΏۂƂ̋���")] public Vector3 offset = new Vector3(0.5f, 0, 0);

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
        playerTrans = player.transform.Find("LookPos").gameObject.transform;
        mainCamera = GameObject.Find("Camera").GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player").gameObject;
            playerTrans = player.transform.Find("LookPos").gameObject.transform;
        }

        // �J�����̉E�x�N�g�����g�p���āAPlayer�̉E���ɔz�u
        Vector3 cameraRight = mainCamera.transform.right;
        Vector3 cameraForward = mainCamera.transform.forward; // �J�����̑O���x�N�g�����擾

        Vector3 targetPosition = playerTrans.position +
                                 cameraRight * offset.x +
                                 Vector3.up * offset.y +
                                 cameraForward * -offset.z; // Z�I�t�Z�b�g���J�����̕����ɓK�p

        transform.position = targetPosition;
    }
}
