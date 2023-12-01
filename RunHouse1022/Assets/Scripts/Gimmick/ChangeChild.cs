using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeChild : MonoBehaviour
{
    private Vector3 lastPosition;
    private CharacterController playerController;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        if (playerController != null)
        {
            Vector3 movementDelta = transform.position - lastPosition;
            playerController.Move(movementDelta);
        }

        lastPosition = transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // Player�I�u�W�F�N�g���G���x�[�^�[�̎q�I�u�W�F�N�g�Ƃ��Đݒ�
            other.transform.parent = this.transform;

            // Player��CharacterController���擾
            playerController = other.gameObject.GetComponent<CharacterController>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // Player�I�u�W�F�N�g�̐e��null�ɐݒ�
            other.transform.parent = null;

            // Player��CharacterController�̎Q�Ƃ����Z�b�g
            playerController = null;
        }
    }
}
