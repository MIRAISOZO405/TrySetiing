using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // �K�v�ȃ��C�u�������C���|�[�g

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MissionManager : MonoBehaviour
{
    public enum MissionType
    {
        CountRandom,        // �b���w��@���������_��
        CountSelect,        // �b���w��@�����w��
        ScheduleRandom,     // �����w��  ���������_��
        ScheduleSelect,     // �����w��  �����w��
    }
    [Header("�~�b�V�����^�C�v"), SerializeField] private MissionType missionType;

    public enum ColorType
    {
        Red,
        Blue,
        Green,
        Yellow,
    }
    [Header("�J���["), SerializeField] private ColorType colorType;

    [Header("�V�O�i���v���n�u")] public GameObject signalPrefab;
    [Header("�~�b�V�����̔�������"), SerializeField] private List<float> invokeTimes = new List<float>();

    [System.Serializable]
    public class MissionData
    {
        public float invokeTime; // �~�b�V�����̔�������
        public GameObject targetPoint; // �ړI�n�ƂȂ�Q�[���I�u�W�F�N�g
    }

    [System.Serializable]
    public class ScheduleSelectMissionData
    {
        public int day;
        public int hour;
        public GameObject targetPoint;
        public bool hasBeenExecuted = false;  // ���̃~�b�V���������s���ꂽ���ǂ����̃t���O
    }

    [System.Serializable]
    public class ScheduleRandomMissionData
    {
        public int day;
        public int hour;
        public bool hasBeenExecuted = false;  // ���̃~�b�V���������s���ꂽ���ǂ����̃t���O
    }

    [Header("�~�b�V�����f�[�^"), SerializeField]
    private List<MissionData> missions = new List<MissionData>();

    [Header("ScheduleSelectd�~�b�V�����f�[�^"), SerializeField]
    private List<ScheduleSelectMissionData> scheduledMissions = new List<ScheduleSelectMissionData>();

    [Header("ScheduleRandom�~�b�V�����f�[�^"), SerializeField]
    private List<ScheduleRandomMissionData> scheduledRandomMissions = new List<ScheduleRandomMissionData>();

    private GameObject missionPrefab; // �~�b�V�����p�̃v���n�u���i�[����ϐ�
    private int missionNo; // �~�b�V�����̔ԍ�������U��
    private Transform parentCanvas;
    private TimeManager timeManager;  // �A�i���O�N���b�N�̎Q�Ƃ�ۑ����邽�߂̕ϐ�

    private void Start()
    {
        missionNo = 1;
        colorType = ColorType.Red;
        parentCanvas = GameObject.Find("MissionCanvas").transform;

        switch (missionType)
        {
            case MissionType.CountRandom:
                foreach (float time in invokeTimes)
                {
                    Invoke("CreateCountRandomSignal", time);
                }
                break;
            case MissionType.CountSelect:
                StartCoroutine(CountSelectMissions());
                break;
            case MissionType.ScheduleRandom:
                timeManager = FindObjectOfType<TimeManager>();
                if (!timeManager)
                {
                    Debug.LogError("TimeManager.cs��������܂���");
                    return;
                }
                StartCoroutine(ScheduleRandomMissions());
                break;
            case MissionType.ScheduleSelect:
                timeManager = FindObjectOfType<TimeManager>();  // �A�i���O�N���b�N�̃C���X�^���X��T��
                if (!timeManager)
                {
                    Debug.LogError("TimeManager.cs��������܂���");
                    return;
                }
                StartCoroutine(ScheduleSelectMissions());
                break;
        }
    }

    private IEnumerator CountSelectMissions()
    {
        float previousInvokeTime = 0.0f;

        for (int i = 0; i < missions.Count; i++)
        {
            float waitTime = missions[i].invokeTime - previousInvokeTime;
            yield return new WaitForSeconds(waitTime);
            CreateScheduleSelectdSignal(missions[i].targetPoint);
            previousInvokeTime = missions[i].invokeTime;
        }
    }

    private IEnumerator ScheduleSelectMissions()
    {
        while (true)
        {
            foreach (ScheduleSelectMissionData mission in scheduledMissions)
            {
                if (!mission.hasBeenExecuted && timeManager.GetDays() == mission.day && timeManager.GetHour() == mission.hour)
                {
                    CreateScheduleSelectdSignal(mission.targetPoint);
                    mission.hasBeenExecuted = true;  // �~�b�V���������s���ꂽ���Ƃ��L�^
                }
            }
            yield return new WaitForSeconds(1);  // ���b�`�F�b�N

            // �z�񏇂Ɍ�������(�������y���������ꍇ��������̗p)
            //for (int i = scheduledMissions.Count - 1; i >= 0; i--)
            //{
            //    var mission = scheduledMissions[i];
            //    if (clock.GetDays() == mission.day && clock.GetHour() == mission.hour)
            //    {
            //        CreateScheduleSelectdSignal(mission.targetPoint);
            //        scheduledMissions.RemoveAt(i);
            //    }
            //}
            //yield return new WaitForSeconds(1);  // ���b�`�F�b�N
        }
    }

    private IEnumerator ScheduleRandomMissions()
    {
        while (true)
        {
            foreach (ScheduleRandomMissionData mission in scheduledRandomMissions)
            {
                if (!mission.hasBeenExecuted && timeManager.GetDays() == mission.day && timeManager.GetHour() == mission.hour)
                {
                    CreateScheduledRandomSignal();
                    mission.hasBeenExecuted = true;
                }
            }
            yield return new WaitForSeconds(1);
        }
    }

    public void CreateCountRandomSignal()
    {
        GameObject signalInstance = Instantiate(signalPrefab, parentCanvas);
        ChangeColor(signalInstance);
        GameObject[] builds = GameObject.FindGameObjectsWithTag("Build");
        if (builds.Length == 0)
        {
            Debug.Log("Build�^�O��������܂���");
            return;
        }
        GameObject randomBuild = builds[Random.Range(0, builds.Length)];
        FacilityMission facilityMission = randomBuild.GetComponent<FacilityMission>();
        if (facilityMission)
        {
            missionPrefab = facilityMission.missionPrefab;
            facilityMission.missionFlg = true;
            facilityMission.missionNo = missionNo;
        }
        SetSignal(signalInstance, randomBuild);
    }

    private void CreateScheduledRandomSignal()
    {
        GameObject signalInstance = Instantiate(signalPrefab, parentCanvas);
        ChangeColor(signalInstance);
        GameObject[] builds = GameObject.FindGameObjectsWithTag("Build");
        if (builds.Length == 0)
        {
            Debug.LogError("�ubuild�v�^�O�̃I�u�W�F�N�g��������܂���");
            return;
        }

        GameObject randomBuild = builds[Random.Range(0, builds.Length)];
        FacilityMission facilityMission = randomBuild.GetComponent<FacilityMission>();
        if (facilityMission)
        {
            missionPrefab = facilityMission.missionPrefab;
            facilityMission.missionFlg = true;
            facilityMission.missionNo = missionNo;
        }
        SetSignal(signalInstance, randomBuild);
    }

    public void CreateScheduleSelectdSignal(GameObject targetObject)
    {
        GameObject signalInstance = Instantiate(signalPrefab, parentCanvas);
        ChangeColor(signalInstance);
        FacilityMission facilityMission = targetObject.GetComponent<FacilityMission>();
        if (facilityMission)
        {
            missionPrefab = facilityMission.missionPrefab;
            facilityMission.missionFlg = true;
            facilityMission.missionNo = missionNo;
        }
        SetSignal(signalInstance, targetObject);
    }

    private void SetSignal(GameObject signalInstance, GameObject targetObject)
    {
        SignalPoint signalPoint = signalInstance.GetComponent<SignalPoint>();
        if (signalPoint)
        {
            signalPoint.SetSignal(targetObject);
            signalPoint.missionNo = missionNo; // �V�O�i���̔ԍ���ݒ�
        }
        else
        {
            Debug.LogError("SignalPoint��������܂���");
        }

        if (parentCanvas)
        {
            Transform missionDisplayTransform = parentCanvas.Find("MissionDisplay");
            SetMissionDisplayColor(missionDisplayTransform, signalInstance);
        }

        missionNo++;
    }

    private void SetMissionDisplayColor(Transform missionDisplayTransform, GameObject signalInstance)
    {
        if (missionDisplayTransform)
        {
            MissionDisplay missionDisplay = missionDisplayTransform.GetComponent<MissionDisplay>();
            if (missionDisplay)
            {
                GameObject missionInstance = missionDisplay.AddMission(missionPrefab, missionNo);
                Transform missionIconTransform = missionInstance.transform.Find("missionIcon");
                if (missionIconTransform)
                {
                    Image missionIcon = missionIconTransform.GetComponent<Image>();
                    if (missionIcon)
                    {
                        missionIcon.color = signalInstance.GetComponentInChildren<Image>().color;
                    }
                    else
                    {
                        Debug.LogError("Image component not found on the missionIcon!");
                    }
                }
                else
                {
                    Debug.LogError("missionIcon child not found under the missionInstance!");
                }
            }
            else
            {
                Debug.LogError("MissionDisplay component not found on the MissionDisplay object!");
            }
        }
        else
        {
            Debug.LogError("MissionDisplay child not found under MissionCanvas!");
        }
    }

    private void ChangeColor(GameObject signalInstance)
    {
        Color targetColor = Color.white; // �f�t�H���g�̐F

        // �Ώۂ�ColorType�Ɋ�Â��ĐF��ݒ�
        switch (colorType)
        {
            case ColorType.Red:
                targetColor = Color.red;
                colorType = ColorType.Blue;
                break;
            case ColorType.Blue:
                targetColor = Color.blue;
                colorType = ColorType.Green;
                break;
            case ColorType.Green:
                targetColor = Color.green;
                colorType = ColorType.Yellow;
                break;
            case ColorType.Yellow:
                targetColor = Color.yellow;
                colorType = ColorType.Red;
                break;
        }

        // ColorList
        // black,clear,cyan,gray,mazenta,white

        // ���ׂĂ̎q��Image��Text�R���|�[�l���g�̐F��ύX
        foreach (Image img in signalInstance.GetComponentsInChildren<Image>())
        {
            img.color = targetColor;
        }

        foreach (Text txt in signalInstance.GetComponentsInChildren<Text>())
        {
            txt.color = targetColor;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MissionManager))]
    private class MissionManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            MissionManager myTarget = (MissionManager)target;

            // �~�b�V�����^�C�v�̃h���b�v�_�E����\��
            myTarget.missionType = (MissionManager.MissionType)EditorGUILayout.EnumPopup("�~�b�V�����^�C�v", myTarget.missionType);

            // �V�O�i���v���n�u�̃t�B�[���h��\��
        myTarget.signalPrefab = (GameObject)EditorGUILayout.ObjectField("Signal Prefab", myTarget.signalPrefab, typeof(GameObject), false);

            // �~�b�V�����^�C�v��CountRandom�̏ꍇ�̂݁AinvokeTimes���X�g��\��
            if (myTarget.missionType == MissionManager.MissionType.CountRandom)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("invokeTimes"), true);
            }
             else if(myTarget.missionType == MissionManager.MissionType.ScheduleSelect)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("scheduledMissions"), true);
            }
            else if (myTarget.missionType == MissionManager.MissionType.ScheduleRandom)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("scheduledRandomMissions"), true);
               }
            else // CountSelect�̏ꍇ�́A�~�b�V�����f�[�^���X�g��\��
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("missions"), true);
            }
           

            serializedObject.ApplyModifiedProperties();  // �ύX��ۑ�
        }
    }
#endif
}

