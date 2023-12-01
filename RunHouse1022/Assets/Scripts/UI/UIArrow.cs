using UnityEngine;
using UnityEngine.UI;

public class UIArrow : MonoBehaviour
{
    private Transform signalPoint;    // ���^�[�Q�b�g
    private Image signalArrow;    // �摜
    private Camera mainCamera;  // �ΏۃJ����

    private void Start()
    {
        signalPoint = transform.parent; // �e���擾
        signalArrow = GetComponent<Image>(); // ���g��Image���擾
        mainCamera = GameObject.Find("Camera").GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        int aspect = Screen.width / Screen.height;  // ���[�g�iCanvas�j�̃X�P�[���l���擾����
        float canvasScale = transform.root.localScale.z;    //root=��ԏ��transfoorm�@�Ȃ����ꍇ���g��transform�@null�͕Ԃ��Ȃ�
        var center = 0.5f * new Vector3(Screen.width, Screen.height);   //canvas�̒��_

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

        float d = Mathf.Max(Mathf.Abs(pos.x / center.x ), Mathf.Abs(pos.y / center.y));//2 �ȏ�̒l����ő�l��Ԃ��܂��B

        // �^�[�Q�b�g�̃X�N���[�����W����ʊO�Ȃ�A��ʒ[�ɂȂ�悤��������(�~�`�ɂ������Ȃ炱���𒲐�����)
        bool isOffscreen = (pos.z < 0f || d > 1f);//d > 1=��ʊO d=�X�N���[�����W
        if (isOffscreen)
        {
            
            pos.x /= d; //pos �̓X�N���[�����W UI�̍��W
            pos.y /= d; //pos.x��pos.y�͉�ʊO�ɍs���ƍ���̂ł��Ŋ�����ē����l���ێ�����B�iif���Œ[�ɍs�����琔�l���Œ肷��f�����������H�j
        }

        signalArrow.rectTransform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg);// ���W�A������x�ɕϊ�����萔

    }
}