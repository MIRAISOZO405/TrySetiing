using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : SingletonMonoBehaviour<CameraManager>
{

    [Header("振動の最大値"), SerializeField] public float maxAmplitude = 3.0f;
    [Header("振動の減少量"), SerializeField] public float downShake = 0.04f;

    private CinemachineVirtualCamera virtualCamera;
    private float minAmplitude = 0.0f;  // 振動の最小値
    private float currentAmplitude;     // 現在の振動量
    private bool shake = false;         // 振動しているかどうか

    private CinemachineBasicMultiChannelPerlin noise;

    private void Start()
    {
        currentAmplitude = maxAmplitude;
        virtualCamera = GameObject.Find("VirtualCamera").GetComponent<CinemachineVirtualCamera>();

        // Virtual CameraからCinemachineBasicMultiChannelPerlinを取得
        noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        if (shake)
        {
            currentAmplitude -= downShake;
            ChangeAmplitude(currentAmplitude);

            if (currentAmplitude <= minAmplitude)
            {
                shake = false;
                currentAmplitude = maxAmplitude;
            }
        }
    }

    public void ShakeCamera()
    {
        if (shake)
            return;

        shake = true;
        ChangeAmplitude(maxAmplitude);
    }

    private void ChangeAmplitude(float amplitude)
    {
        // 振幅の範囲を0.0から1.0に正規化
        float normalizedAmplitude = Mathf.Lerp(minAmplitude, maxAmplitude, amplitude);

        // Virtual CameraのNoiseの振幅を設定
        noise.m_AmplitudeGain = normalizedAmplitude;
    }
}
