using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [Header("1���̒���(��)"), Tooltip("Unity���24���Ԃ������̉����ɐݒ肷�邩"), SerializeField]
    public float dayDurationInMinutes = 2f; // �C���X�y�N�^�[�����]����1���̒����𕪒P�ʂŐݒ�
    [Header("��������"), Tooltip("�����ŃQ�[���I�����邩"), SerializeField]
    private int limitDays = 7;

    [Header("�J�n����")] public int startHour = 9; // �C���X�y�N�^�[����0�`23�̊ԂŃQ�[���J�n���̎��Ԃ�ݒ�
    [Header("�o�ߓ���"), SerializeField] private int elapsedDays = 0;

    [Header("����"), SerializeField] private int currentHour;
    [Header("����"), SerializeField] private int currentMinute;

    [Space]
    public GameObject minute; // ���j�p
    public GameObject hour;  // ���j�p

    private float timeMultiplier; // �����ԂƎw�莞�Ԃ̕ϊ���
    private float startTimeInMinutes; // �Q�[���J�n���̎��ۂ̎���

    private RentalIncome rentalIncome;    // �ƒ������p

    private void Start()
    {
        // 1���̎����ԁi24���ԁj�Ǝw�莞�ԂƂ̕ϊ������v�Z
        timeMultiplier = 24 * 60 / dayDurationInMinutes;

        // �Q�[���J�n���̎��ۂ̎��Ԃ�ۑ�
        startTimeInMinutes = DateTime.Now.Hour * 60 + DateTime.Now.Minute + DateTime.Now.Second / 60f + DateTime.Now.Millisecond / 60000f;

        rentalIncome = FindObjectOfType<RentalIncome>();
    }

    private void Update()
    {
        // ���݂̌o�ߎ��Ԃ��擾 (���P��)
        float currentRealMinutesPassed = DateTime.Now.Hour * 60 + DateTime.Now.Minute + DateTime.Now.Second / 60f + DateTime.Now.Millisecond / 60000f;

        // �Q�[���J�n������̎����Ԃ̌o�߂��v�Z
        float elapsedRealMinutes = currentRealMinutesPassed - startTimeInMinutes;

        // �Q�[�����ł̌o�ߎ��Ԃ��v�Z
        float gameMinutesPassed = (startHour * 60) + (elapsedRealMinutes * timeMultiplier);

        // dayDurationInMinutes�ȏ㌻���̎��Ԃ��o�߂����ꍇ�A�o�ߓ����̃J�E���g�A�b�v
        if (elapsedRealMinutes >= dayDurationInMinutes)
        {
            elapsedDays++;

            if (elapsedDays >= limitDays)
            {
                // �Q�[���I��
                FadeManager.Instance.LoadScene("TitleScene");
            }

            // �ƒ����Ϗ���
            rentalIncome.RentPayment();

            startTimeInMinutes = currentRealMinutesPassed;
            startHour = 9; // �Q�[���̊J�n������9���Ƀ��Z�b�g
        }

        // 24���Ԃ̃��Z�b�g�̂���
        gameMinutesPassed %= (24 * 60);

        // ���݂̃Q�[�����̎������X�V
        currentHour = Mathf.FloorToInt(gameMinutesPassed / 60);
        currentMinute = Mathf.FloorToInt(gameMinutesPassed % 60);

        // �j�̊p�x���v�Z
        hour.transform.eulerAngles = new Vector3(0, 0, gameMinutesPassed / (12 * 60) * -360);
        minute.transform.eulerAngles = new Vector3(0, 0, gameMinutesPassed / 60 * -360);
    }


    public int GetDays()
    {
        return elapsedDays;
    }

    public int GetHour()
    {
        return currentHour;
    }

    public int GetMinute()
    {
        return currentMinute;
    }
}
