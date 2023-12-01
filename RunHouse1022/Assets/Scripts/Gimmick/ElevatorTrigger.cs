using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    private Elevator elevatorScript;

    private void Start()
    {
        elevatorScript = GetComponentInParent<Elevator>(); // �e�I�u�W�F�N�g��Erevator�X�N���v�g���擾
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            elevatorScript.PlayerEnteredElevator(); // �v���C���[��������̂����m�����̂ŁA���C���̃X�N���v�g�̃��\�b�h���Ăяo��
        }
    }
}
