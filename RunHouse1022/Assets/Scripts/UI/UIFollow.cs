using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollow : MonoBehaviour
{
    private GameObject player;
    private Transform playerTrans;
    private Camera mainCamera;
    [Header("対象との距離")] public Vector3 offset = new Vector3(0.5f, 0, 0);

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

        // カメラの右ベクトルを使用して、Playerの右側に配置
        Vector3 cameraRight = mainCamera.transform.right;
        Vector3 cameraForward = mainCamera.transform.forward; // カメラの前方ベクトルを取得

        Vector3 targetPosition = playerTrans.position +
                                 cameraRight * offset.x +
                                 Vector3.up * offset.y +
                                 cameraForward * -offset.z; // Zオフセットをカメラの方向に適用

        transform.position = targetPosition;
    }
}
