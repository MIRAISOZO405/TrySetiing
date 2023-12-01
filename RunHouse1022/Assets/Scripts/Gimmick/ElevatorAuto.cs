using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorAuto : MonoBehaviour
{
    private Transform elevator;
    private Transform pos1;
    private Transform pos2;

    [Header("�ړ��X�s�[�h"), SerializeField]
    private float speed = 2.0f; // �ړ����x
    private float journeyLength; // �ړ�����
    private float startTime; // �ړ��J�n����
    private bool movingToPos2 = true; // pos2�Ɍ������Ă��邩�̃t���O

    private Vector3 lastElevatorPosition;

    [Header("�G���x�[�^�[��~����"), SerializeField]
    private float waitTime = 2.0f; // �ǉ�: ��~���Ԃ�2�b�ɐݒ�
    private float currentWaitTime = 0.0f; // �ǉ�: ���݂̒�~����

    void Start()
    {
        elevator = transform.Find("Elevator").gameObject.transform;
        pos1 = transform.Find("pos1");
        pos2 = transform.Find("pos2");

        if (elevator == null || pos1 == null || pos2 == null)
            Debug.LogWarning("�q�I�u�W�F�N�g������Ȃ�");

        elevator.position = pos1.position; // �ǉ�: Erevator�̈ʒu��pos1�̈ʒu�ɐݒ�

        pos1.gameObject.SetActive(false);
        pos2.gameObject.SetActive(false);

        startTime = Time.time;
        journeyLength = Vector3.Distance(pos1.position, pos2.position);

        lastElevatorPosition = elevator.position;
    }

    void Update()
    {
        Vector3 currentErevatorPosition = elevator.position;
        Vector3 erevatorMovement = currentErevatorPosition - lastElevatorPosition;

        // �G���x�[�^�[����~���̏ꍇ�A��~���Ԃ�����������
        if (currentWaitTime > 0)
        {
            currentWaitTime -= Time.deltaTime;
            lastElevatorPosition = elevator.position;
            return; // ������Update���I��
        }

        // �ړ��̐i�s�����v�Z
        float distanceCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distanceCovered / journeyLength;

        if (movingToPos2)
        {
            elevator.position = Vector3.Lerp(pos1.position, pos2.position, fractionOfJourney);

            if (fractionOfJourney >= 1)
            {
                movingToPos2 = false;
                startTime = Time.time + waitTime; // �ǉ�: ���̓�����҂����Ԃ����x�点��
                currentWaitTime = waitTime; // �ǉ�: ��~���Ԃ����Z�b�g
            }
        }
        else
        {
            elevator.position = Vector3.Lerp(pos2.position, pos1.position, fractionOfJourney);

            if (fractionOfJourney >= 1)
            {
                movingToPos2 = true;
                startTime = Time.time + waitTime; // �ǉ�: ���̓�����҂����Ԃ����x�点��
                currentWaitTime = waitTime; // �ǉ�: ��~���Ԃ����Z�b�g
            }
        }

        lastElevatorPosition = elevator.position;
    }
}
