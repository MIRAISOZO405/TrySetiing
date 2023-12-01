using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using PlayerEnums;

public class MissionDisplay : MonoBehaviour
{
    // �~�b�V���������i�[����N���X�̒�`
    [System.Serializable]
    public class MissionInfo
    {
        public int missionNo;
        public GameObject missionPrefab;
        public GameObject missionInstance;
     
        public MissionInfo(int no, GameObject prefab)
        {
            missionNo = no;
            missionPrefab = prefab;
            missionInstance = null;
        }
    }

    // ��Ԉړ������邽�߂̍\����
    private struct MovingMission
    {
        public GameObject missionObject;
        public Vector3 targetPosition;
        public Vector3 startPosition;
        public float timeToMove;
        public float elapsedTime;
    }
    private List<MovingMission> movingMissions = new List<MovingMission>();

    [Header("Text�̏����ʒu")] 
    public Vector3 initialPosition = new Vector3(0, 0, 0); // �����ʒu

    [Header("Text�̊Ԋu")] 
    public float interval = -50f; // Y���W�̊Ԋu

    [Header("��Ԉړ��̑��x"),Tooltip("�������قǑ���")]
    public float interpolationSpeed = 0.5f;  // 0.5�b�i�f�t�H���g�j�ňړ�����

    [Space,Header("�~�b�V�����ꗗ"),SerializeField]
    private List<MissionInfo> missions = new List<MissionInfo>();   // �~�b�V���������i�[���郊�X�g

    [Header("���s���̌��_")]
    public int failureScore = -0;

    private ScoreManager scoreScript;

    private void Start()
    {
        GameObject score = GameObject.Find("Score").gameObject;

        if (score)
        {
            scoreScript = score.GetComponent<ScoreManager>();
            if (!scoreScript)
                Debug.LogError("ScoreManager�R���|�[�l���g��������܂���");
        }
        else
        {
            Debug.LogError("Score�I�u�W�F�N�g��������܂���");
        }
    }

    public GameObject AddMission(GameObject missionPrefab, int missionNo)
    {
        if (missionPrefab)
        {
            // �w�肵���ʒu�Ńv���n�u���C���X�^���X�����A���̎Q�Ƃ�MissionInfo�Ɋi�[
            GameObject instance = Instantiate(missionPrefab, transform);

            // �������ꂽ�C���X�^���X����Mission�X�N���v�g���擾���AmissionNo��ݒ�
            UISignalLimit missionScript = instance.GetComponent<UISignalLimit>();
            if (missionScript)
                missionScript.missionNo = missionNo;

            MissionInfo newMission = new MissionInfo(missionNo, missionPrefab);
            newMission.missionInstance = instance;
            missions.Add(newMission);

            // �V�����~�b�V�����𒼐ږړI�̈ʒu�ɔz�u
            Vector3 targetPosition = initialPosition + new Vector3(0, interval * (missions.Count - 1), 0); // �V�����~�b�V�����̈ʒu
            newMission.missionInstance.transform.localPosition = targetPosition;

            return instance; // �������ꂽ�~�b�V�����̃C���X�^���X��Ԃ�
        }
        else
        {
            Debug.LogError("missionPrefab��������܂���");
            return null;
        }
    }

    private void Update()
    {
        for (int i = 0; i < movingMissions.Count; i++)
        {
            var movingMission = movingMissions[i];
            movingMission.elapsedTime += Time.deltaTime;
            float ratio = Mathf.Clamp01(movingMission.elapsedTime / movingMission.timeToMove);

            if (movingMission.missionObject != null)
            {
                movingMission.missionObject.transform.localPosition = Vector3.Lerp(movingMission.startPosition, movingMission.targetPosition, ratio);
            }
            else
            {
                Debug.LogError("movingMission.missionObject��������܂���");
            }
          
            if (ratio >= 1)
            {
                movingMissions.RemoveAt(i);
                i--;  // ���X�g�̗v�f���폜�������߁A�C���f�b�N�X���f�N�������g
            }
            else
            {
                movingMissions[i] = movingMission;
            }
        }
    }

    private void UpdateMissionPositions()
    {
        for (int i = 0; i < missions.Count; i++)
        {
            Vector3 targetPosition = initialPosition + new Vector3(0, interval * i, 0);

            // ��Ԉړ��̂��߂̏���ݒ�
            MovingMission movingMission = new MovingMission
            {
                missionObject = missions[i].missionInstance,
                startPosition = missions[i].missionInstance.transform.localPosition,
                targetPosition = targetPosition,
                timeToMove = 0.5f,  // ���̒l��ύX���Ĉړ��ɂ����鎞�Ԃ𒲐����܂�
                elapsedTime = 0
            };
            movingMissions.Add(movingMission);
        }
    }

    public void ClearMission(int missionNo,int point)
    {
        if (RemoveMission(missionNo))
        {
            if (scoreScript)
                scoreScript.AddScore(point);
        }
    }

    public void FailureMission(int missionNo)
    {
        if (RemoveMission(missionNo))
        {
            if (scoreScript)
                scoreScript.AddScore(failureScore);
        }
    }

    private bool RemoveMission(int missionNo)
    {
        MissionInfo missionToRemove = missions.Find(m => m.missionNo == missionNo);

        if (missionToRemove == null)
        {
            Debug.LogWarning($"Mission with number {missionNo} not found in the list!");
            return false;
        }

        missions.Remove(missionToRemove);

        if (missionToRemove.missionInstance != null)
        {
            Destroy(missionToRemove.missionInstance);
        }

        GameObject[] signals = GameObject.FindGameObjectsWithTag("Signal");
        foreach (GameObject signal in signals)
        {
            SignalPoint signalPoint = signal.GetComponent<SignalPoint>();
            if (signalPoint != null && signalPoint.missionNo == missionNo)
            {
                Destroy(signal);
                UpdateMissionPositions();
                return true;
            }
        }
        return false;
    }

}



