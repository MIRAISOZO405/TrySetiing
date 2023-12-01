using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class SignalPoint : MonoBehaviour
{
    [Header("�V�O�i���������̃I�u�W�F�N�g"), SerializeField] public Transform signalPoint;  //�^�[�Q�b�g   
    public int missionNo = 0;
    private Image signalArrow; //�摜
    private Camera mainCamera;  //�ΏۃJ����
    private RectTransform rectTransform;
    private RectTransform textRectTransform;

    private Transform player;   // Player�̈ʒu
    private Text distanceText; // ������\������UI�e�L�X�g

    private GameObject uiElement; // �؂�ւ�����UI�v�f
    private GameObject targetObj;
    private Vector3 textPos;

    public float radius = 10.0f; //�~�̔��a

    private void Start()
    {
        signalArrow = transform.Find("SignalArrow").GetComponent<Image>();  // �q��image���擾
        uiElement = transform.Find("SignalArrow").gameObject;
        targetObj = transform.Find("SignalArrow").gameObject;
        distanceText = transform.Find("distanceText").GetComponent<Text>(); // �q��Text���擾

        // �s�x�؂�ւ���K�v����
        player = GameObject.FindGameObjectWithTag("Player").transform;   // Plyer���擾
        mainCamera = GameObject.Find("Camera").GetComponent<Camera>();
        rectTransform = GetComponent<RectTransform>();
        textRectTransform = distanceText.GetComponent<RectTransform>();
    }

    private void Update()
    {
        // �I�u�W�F�N�g�Ԃ̋������v�Z
        float distance = Vector3.Distance(player.position, signalPoint.position);

        // ������UI�e�L�X�g�ɕ\��
        distanceText.text = distance.ToString("F2") + "M"; // 2���̏����_�܂ŕ\��
    }


    private void LateUpdate()
    {
        // Player���f�����`�F���W���āAPlayer�����������Ƃ�
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;   // Plyer���擾
        }

        if (!signalPoint)
        {
            return;
        }

        int aspect = Screen.width / Screen.height;
        // ���[�g�iCanvas�j�̃X�P�[���l���擾����
        float canvasScale = transform.root.localScale.z;//root=��ԏ��transfoorm�@�Ȃ����ꍇ���g��transform�@null�͕Ԃ��Ȃ�
        var center = 0.5f * new Vector3(Screen.width, Screen.height);//canvas�̒��_

        // �i��ʒ��S�����_(0,0)�Ƃ����j�^�[�Q�b�g�̃X�N���[�����W�����߂�
        var pos = mainCamera.WorldToScreenPoint(signalPoint.position) - center;
        if (pos.z < 0f)//�^�[�Q�b�g���J�����̌���ɂ�������
        {
            pos.x = -pos.x; // �J��������ɂ���^�[�Q�b�g�̃X�N���[�����W�́A��ʒ��S�ɑ΂���_�Ώ̂̍��W�ɂ���
            pos.y = -pos.y;

            // �J�����Ɛ����ȃ^�[�Q�b�g�̃X�N���[�����W��␳����
            if (Mathf.Approximately(pos.y, 0f))//2�� float ���r���A����炪�݂��ɏ����Ȓl (Epsilon) �͈͓̔��ɂ���ꍇ true ��Ԃ��܂��B
            {
                pos.y = -center.y;
            }
        }

        //��ʒ[�̕\���ʒu��UI�̃T�C�Y�̔���������ʒ��S���Ɋ񂹂āA��ʒ[��UI�����؂�Ȃ��悤�ɂ��܂��B
        var halfSize = 1.0f * canvasScale * rectTransform.sizeDelta; // UI���W�n�̒l���X�N���[�����W�n�̒l�ɕϊ�����

        float d = Mathf.Max( Mathf.Abs(pos.x / (center.x - halfSize.x)),Mathf.Abs(pos.y / (center.y - halfSize.y)));//2 �ȏ�̒l����ő�l��Ԃ��܂��B

        // �^�[�Q�b�g�̃X�N���[�����W����ʊO�Ȃ�A��ʒ[�ɂȂ�悤��������(�~�`�ɂ������Ȃ炱���𒲐�����)
        bool isOffscreen = (pos.z < 0f || d > 1f);//d > 1=��ʊO d=�X�N���[�����W
        if (isOffscreen)
        {
            //�����ł���Ă��邱�Ƃ�UI�̈ʒu�ipos.x��pos.y���X�N���[���̊O�ɍs���Ȃ��悤�ɒl�𐧌����Ă���
            //�~�`�ɂ���ɂ�pos.x��pos.y�̒l���~��`���悤�ɐ�������΂悢
            // Mathf.Pow(pos.x - (Screen.width / 2),2) + Mathf.Pow(pos.y - (Screen.height / 2),2) = Mathf.Pow(radius, 2); 

            pos.x /= d;//pos �̓X�N���[�����W UI�̍��W
            pos.y /= d;//pos.x��pos.y�͉�ʊO�ɍs���ƍ���̂ł��Ŋ�����ē����l���ێ�����B�iif���Œ[�ɍs�����琔�l���Œ肷��f�����������H�j
            float dx = (pos.x / (center.x - halfSize.x));
            float dy = (pos.y / (center.y - halfSize.y));

            if (dx >= 1) // �E
            {
                textPos.x = -61;
                textPos.y = -37;
            }
            if (dx <= -1) // ��
            {
                textPos.x = 65;
                textPos.y = -33;
            }
            if (dy >= 1.0) // ��
            {
                textPos.x = 4;
                textPos.y = -60;
            }
            if (dy < -1) // ��
            {
                textPos.x = 3;
                textPos.y = -8;
            }

            uiElement.SetActive(true);
        }
        else
        {
            textPos.x = 20;
            textPos.y = -9;
            uiElement.SetActive(false); // �\��/��\����؂�ւ���
        }

        SetTextPosition();

        // �X�N�������W�n�̒l��UI���W�n�̒l�ɕϊ�����
        rectTransform.anchoredPosition = pos / canvasScale;
    }

    private void SetTextPosition()
    {
        float canvasScale = transform.root.localScale.z;    // root=��ԏ��transfoorm�@�Ȃ����ꍇ���g��transform�@null�͕Ԃ��Ȃ�
        textRectTransform.localPosition = textPos;  // canvasScale;
    }

    public void SetSignal(GameObject target)
    {
        signalPoint = target.transform;
    }
}
