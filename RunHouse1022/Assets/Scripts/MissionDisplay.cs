using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using PlayerEnums;

public class MissionDisplay : MonoBehaviour
{
    // ミッション情報を格納するクラスの定義
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

    // 補間移動させるための構造体
    private struct MovingMission
    {
        public GameObject missionObject;
        public Vector3 targetPosition;
        public Vector3 startPosition;
        public float timeToMove;
        public float elapsedTime;
    }
    private List<MovingMission> movingMissions = new List<MovingMission>();

    [Header("Textの初期位置")] 
    public Vector3 initialPosition = new Vector3(0, 0, 0); // 初期位置

    [Header("Textの間隔")] 
    public float interval = -50f; // Y座標の間隔

    [Header("補間移動の速度"),Tooltip("小さいほど速い")]
    public float interpolationSpeed = 0.5f;  // 0.5秒（デフォルト）で移動する

    [Space,Header("ミッション一覧"),SerializeField]
    private List<MissionInfo> missions = new List<MissionInfo>();   // ミッション情報を格納するリスト

    [Header("失敗時の減点")]
    public int failureScore = -0;

    private ScoreManager scoreScript;

    private void Start()
    {
        GameObject score = GameObject.Find("Score").gameObject;

        if (score)
        {
            scoreScript = score.GetComponent<ScoreManager>();
            if (!scoreScript)
                Debug.LogError("ScoreManagerコンポーネントが見つかりません");
        }
        else
        {
            Debug.LogError("Scoreオブジェクトが見つかりません");
        }
    }

    public GameObject AddMission(GameObject missionPrefab, int missionNo)
    {
        if (missionPrefab)
        {
            // 指定した位置でプレハブをインスタンス化し、その参照をMissionInfoに格納
            GameObject instance = Instantiate(missionPrefab, transform);

            // 生成されたインスタンスからMissionスクリプトを取得し、missionNoを設定
            UISignalLimit missionScript = instance.GetComponent<UISignalLimit>();
            if (missionScript)
                missionScript.missionNo = missionNo;

            MissionInfo newMission = new MissionInfo(missionNo, missionPrefab);
            newMission.missionInstance = instance;
            missions.Add(newMission);

            // 新しいミッションを直接目的の位置に配置
            Vector3 targetPosition = initialPosition + new Vector3(0, interval * (missions.Count - 1), 0); // 新しいミッションの位置
            newMission.missionInstance.transform.localPosition = targetPosition;

            return instance; // 生成されたミッションのインスタンスを返す
        }
        else
        {
            Debug.LogError("missionPrefabが見つかりません");
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
                Debug.LogError("movingMission.missionObjectが見つかりません");
            }
          
            if (ratio >= 1)
            {
                movingMissions.RemoveAt(i);
                i--;  // リストの要素を削除したため、インデックスをデクリメント
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

            // 補間移動のための情報を設定
            MovingMission movingMission = new MovingMission
            {
                missionObject = missions[i].missionInstance,
                startPosition = missions[i].missionInstance.transform.localPosition,
                targetPosition = targetPosition,
                timeToMove = 0.5f,  // この値を変更して移動にかかる時間を調整します
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



