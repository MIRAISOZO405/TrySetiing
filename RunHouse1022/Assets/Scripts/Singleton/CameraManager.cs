using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : SingletonMonoBehaviour<CameraManager>
{

    [Header("�U���̍ő�l"), SerializeField] public float maxAmplitude = 3.0f;
    [Header("�U���̌�����"), SerializeField] public float downShake = 0.04f;

    private CinemachineVirtualCamera virtualCamera;
    private float minAmplitude = 0.0f;  // �U���̍ŏ��l
    private float currentAmplitude;     // ���݂̐U����
    private bool shake = false;         // �U�����Ă��邩�ǂ���

    private CinemachineBasicMultiChannelPerlin noise;

    private void Start()
    {
        currentAmplitude = maxAmplitude;
        virtualCamera = GameObject.Find("VirtualCamera").GetComponent<CinemachineVirtualCamera>();

        // Virtual Camera����CinemachineBasicMultiChannelPerlin���擾
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
        // �U���͈̔͂�0.0����1.0�ɐ��K��
        float normalizedAmplitude = Mathf.Lerp(minAmplitude, maxAmplitude, amplitude);

        // Virtual Camera��Noise�̐U����ݒ�
        noise.m_AmplitudeGain = normalizedAmplitude;
    }
}
