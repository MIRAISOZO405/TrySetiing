using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    private Transform elevator;
    private Transform pos1;
    private Transform pos2;

    [Header("�ړ��X�s�[�h"), SerializeField]
    private float speed = 2.0f;
    private float journeyLength;
    private float startTime;
    private bool movingToPos2 = true;

    private Vector3 lastElevatorPosition;

    [Header("�G���x�[�^�[�J�n�ҋ@����"), SerializeField]
    private float startDelay = 1.0f; // �J�n�O�̑ҋ@����
    private float currentStartDelay = 0.0f;

    private float currentWaitTime = 0.0f;

    private bool playerOnElevator = false;

    void Start()
    {
        elevator = transform.Find("Elevator").gameObject.transform;
        pos1 = transform.Find("pos1");
        pos2 = transform.Find("pos2");

        if (elevator == null || pos1 == null || pos2 == null)
            Debug.LogWarning("�q�I�u�W�F�N�g������Ȃ�");

        elevator.position = pos1.position;

        pos1.gameObject.SetActive(false);
        pos2.gameObject.SetActive(false);

        startTime = Time.time;
        journeyLength = Vector3.Distance(pos1.position, pos2.position);

        lastElevatorPosition = elevator.position;
    }

    void Update()
    {
        if (currentWaitTime > 0)
        {
            currentWaitTime -= Time.deltaTime;
            return;
        }

        if (playerOnElevator && currentStartDelay <= 0) // �v���C���[���G���x�[�^�[�ɏ���Ă��āA�J�n�ҋ@���Ԃ��o�߂����ꍇ
        {
            float distanceCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distanceCovered / journeyLength;

            if (movingToPos2)
            {
                elevator.position = Vector3.Lerp(pos1.position, pos2.position, fractionOfJourney);

                if (fractionOfJourney >= 1)
                {
                    movingToPos2 = false;
                    playerOnElevator = false;
                    startTime = Time.time;
                }
            }
            else
            {
                elevator.position = Vector3.Lerp(pos2.position, pos1.position, fractionOfJourney);

                if (fractionOfJourney >= 1)
                {
                    movingToPos2 = true;
                    playerOnElevator = false;
                    startTime = Time.time;
                }
            }
        }
        else if (playerOnElevator) // �v���C���[���G���x�[�^�[�ɏ���Ă��邪�A�J�n�ҋ@���Ԃ��܂��o�߂��Ă��Ȃ��ꍇ
        {
            currentStartDelay -= Time.deltaTime;
            if (currentStartDelay <= 0)
            {
                startTime = Time.time; // �J�n�ҋ@���Ԃ��o�߂����̂ŁA�ړ����J�n����
            }
        }
    }

    public void PlayerEnteredElevator()
    {
        if (!playerOnElevator)
        {
            playerOnElevator = true;
            currentStartDelay = startDelay; // �J�n�ҋ@���Ԃ�ݒ�
        }
    }
}
