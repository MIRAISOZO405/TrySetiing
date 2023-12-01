using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchAnim : MonoBehaviour
{
    private GameObject switchButton;
    private Animation anim;

    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 targetPosition;
    private Vector3 arcHeightVector;

    public float maxArcHeight = 5f;
    public float minArcHeight = 2f;
    public float speed = 10f;
    private float time = 0f;

    private CharacterController charController; // �L�����N�^�[�R���g���[���[
    private GameObject objectToMove; // ��΂��I�u�W�F�N�g
    private bool isMoving = false;
    public bool isRiding = false;
    public bool isLock = false;

    public SwitchAnim otherSwitch; // ���̃X�C�b�`�ւ̎Q��

    void Start()
    {
        // �q����SwitchButton���擾
        switchButton = transform.Find("SwitchButton").gameObject;
        anim = gameObject.GetComponent<Animation>();

        objectToMove = GameObject.FindWithTag("Player");
        charController = objectToMove.GetComponent<CharacterController>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (isLock)
                return;

            isRiding = true;
            anim.Play();      
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isRiding = false;

            if (isLock)
                isLock = false;
        }
    }


    public void OnLaunch()
    {
        //�A�j���[�V�����I�����̏���
        if (!isRiding)
            return;

        //objectToMove = GameObject.FindWithTag("Player");
        //charController = objectToMove.GetComponent<CharacterController>();

        if (objectToMove == null)
        {
            objectToMove = GameObject.FindWithTag("Player");
            charController = objectToMove.GetComponent<CharacterController>();
        }

        time = 0;
        startPosition = objectToMove.transform.position;

        // 3������Ԃł̋������v�Z���܂��B
        float distance = Vector3.Distance(startPosition, targetPosition);

        // distance�̒l��0����150�͈̔͂Ɏ��܂�悤�ɂ��܂��B
        distance = Mathf.Clamp(distance, 0, 100);

        // distance��0����150�͈̔͂Ɋ�Â���arcHeightVector��minArcVector����maxArcVector�ɐ��`��Ԃ��܂��B
        float lerpAmount = distance / 100f;
        arcHeightVector = Vector3.up * Mathf.Lerp(maxArcHeight, minArcHeight, lerpAmount);

        isMoving = true; // �ړ����J�n
        otherSwitch.isLock = true;
       
    }

    public void SetTargetPosition(GameObject target)
    {
        otherSwitch = target.GetComponent<SwitchAnim>();
        targetPosition = target.transform.position;
    }

    private void Update()
    {
        if (isMoving && charController)
        {
            time += Time.deltaTime;

            float distanceCovered = time * speed;
            Vector3 direction = (targetPosition - startPosition).normalized;
            Vector3 nextPos = startPosition + direction * distanceCovered;
            nextPos.y += arcHeightVector.y * Mathf.Sin(Mathf.Clamp01(distanceCovered / Vector3.Distance(startPosition, targetPosition)) * Mathf.PI);

            // �L�����N�^�[�R���g���[�����g�p���ăv���C���[���ړ�
            Vector3 moveVector = nextPos - objectToMove.transform.position;
            charController.Move(moveVector);

            // �ڕW�ʒu�ɓ��B������A�ړ����I��
            if (Vector3.Distance(objectToMove.transform.position, targetPosition) <= 1.0f)
            {
                isMoving = false;
                //charController = null;
                //objectToMove = null;
            }

        }
    }
}
