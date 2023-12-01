using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScoreManager : MonoBehaviour
{
    [Header("fillAmount�̒l"), SerializeField] private float fill = 0f;
    [Header("���݃X�R�A"), SerializeField] private int currentScore = 0;
    [Header("�ő�X�R�A"), SerializeField] private int maxScore = 100;
    [Header("�A�j���[�V������������"), SerializeField] private float durationTime = 1f;
    private int displayCurrentScore;
    private int displayMaxScore;

    private Image bar;
    private Text text;


    private void Start()
    {
        bar = transform.Find("ScoreBar").GetComponent<Image>();
        if (bar)
            bar.fillAmount = fill;
        else
            Debug.LogError("ScoreBar��������܂���");

        text = transform.Find("ScoreText").GetComponent<Text>();
        if (text)
            text.text = currentScore + "/" + maxScore;
        else
            Debug.LogError("ScoreText��������܂���");

    }

    void Update()
    {
        // ���Ƃŏ����i���f���`�F���W�j
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AddScore(10);
        }
    }

    public void AddScore(int score)
    {
        currentScore += score;  // �X�R�A��ǉ�

        // �X�R�A���ő�
        if (currentScore >= maxScore)
        {
            currentScore = maxScore;
        }

        // �X�R�A��0�����ɂȂ�Ȃ��悤�ɂ���
        if (currentScore < 0)
            currentScore = 0;

        // ���݂̃X�R�A����ŏI�I�ȃX�R�A�܂ł̐������A�j���[�V����������
        DOTween.To(() => displayCurrentScore, x =>
        {
            displayCurrentScore = x; // �\���p���l���X�V
            text.text = displayCurrentScore + "/" + maxScore; // �e�L�X�g���X�V

        }, currentScore, durationTime);

        UpdateFillAmount();
    }

    public void SetMaxScore(int max)
    {
        maxScore = max;

        DOTween.To(() => displayMaxScore, x =>
        {
            displayMaxScore = x;
            text.text = currentScore + "/" + displayMaxScore;

        }, maxScore, durationTime);

        UpdateFillAmount();
    }

    private void UpdateFillAmount()
    {
        // fill�̐V�����l���v�Z
        float newFillValue = (float)currentScore / maxScore;

        // �o�[��fillAmount���A�j���[�V����������
        DOTween.To(() => bar.fillAmount, x => bar.fillAmount = x, newFillValue, durationTime);
    }

    public int GetScore()
    {
        return currentScore;
    }
}
