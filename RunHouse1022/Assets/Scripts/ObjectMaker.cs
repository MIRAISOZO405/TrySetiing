using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMaker : MonoBehaviour
{
    [Header("オブジェクト生成確率(100%中)")] public int probability;
    [Header("使用するプレハブの種類")] public int preNum = 3;
    [Header("使用するBlockをアタッチ"), SerializeField] private ObjectArray objectArray;

    [System.Serializable]
    public class ObjectArray
    {
        public GameObject[] objectPrefab;
    }

    [Header("ブロック数(横)")] public int xNum;
    [Header("ブロック数(奥)")] public int zNum;
    [Header("スタート位置")] public Vector3 startPos = new Vector3(0, 0, 0);
    [Header("ブロックの大きさ")] public float unitWidth = 1;
    [Header("クリックでオブジェクト生成")] public bool create;
    [Space]
    public List<GameObject> objects;

    private void OnValidate()
    {
        if (objectArray.objectPrefab == null || objectArray.objectPrefab.Length != preNum)
        {
            objectArray.objectPrefab = new GameObject[preNum];
        }

        if (create)
        {
            create = false;

            // 40は最大値
            zNum = Mathf.Min(40, zNum);
            xNum = Mathf.Min(40, xNum);
            int count = objects.Count;

            var blockBase = new GameObject("Objects");
            blockBase.transform.SetParent(transform);

            objects.Clear();
            float posZ = startPos.z;
            float posY = startPos.y;
            float posX = startPos.x;

            for (int z = 0; z < zNum; z++)
            {
                for (int x = 0; x < xNum; x++)
                {
                    // 0～99のランダムな数を取得して、その数がprobability未満の場合のみオブジェクトを生成
                    if (Random.Range(0, 100) < probability)
                    {
                        int index = Random.Range(0, preNum);


                        GameObject prefab = objectArray.objectPrefab[index];

                        GameObject birthObj = Instantiate(prefab);
                        birthObj.transform.position = new Vector3(posX, posY, posZ);
                        birthObj.transform.SetParent(blockBase.transform);
                        birthObj.name = "Object" + " x:" + x + " z:" + z;

                        objects.Add(birthObj);
                    }
                    posX += unitWidth;
                }
                posZ += unitWidth;
                posX = startPos.x;
            }
        }
    }
}
