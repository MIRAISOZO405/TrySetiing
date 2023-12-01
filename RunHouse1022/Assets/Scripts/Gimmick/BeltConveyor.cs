using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltConveyor : MonoBehaviour
{
    private CharacterController characterController;
    public float moveSpeed;
    private bool isRiding; // ����Ă��邩�ǂ���


    private void OnTriggerEnter(Collider other)
    {
        // �ȑO��ConveyorBelt��use���I�t�ɂ���
        if (other.gameObject.GetComponent<PlayerController>().currentConveyor)
        {
            other.gameObject.GetComponent<PlayerController>().currentConveyor.isRiding = false;
        }

        isRiding = true;
        characterController = other.gameObject.GetComponent<CharacterController>();

        // �V����ConveyorBelt�����݂�Conveyor�Ƃ��Đݒ�
        other.gameObject.GetComponent<PlayerController>().currentConveyor = this;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isRiding = false;
            characterController = null;
        }
    }

    private void Update()
    {
        ScrollUV();

        if (!isRiding)
            return;

        Vector3 localMovement = new Vector3(0, 0, moveSpeed);
        Vector3 worldMovement = transform.TransformDirection(localMovement);
        characterController.Move(worldMovement * Time.deltaTime);
    }

    void ScrollUV()
    {
        var material = GetComponent<Renderer>().material;
        Vector2 offset = material.mainTextureOffset;
        offset += Vector2.up * moveSpeed * Time.deltaTime;
        material.mainTextureOffset = offset;
    }
}
