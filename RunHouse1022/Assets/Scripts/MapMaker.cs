using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMaker : MonoBehaviour
{
    enum BlockType
    {
        Single,    // 1種類のブロックを配置
        Cross,       // 2種類のブロックをタイル状に配置
        Random,     // 複数種類のブロックをランダム配置
    }
    [Header("配置タイプ"), SerializeField] private BlockType blockType;

    [Header("使用するプレハブの種類(RundomType)")] public int preNum = 3;
    [Header("使用するBlockをアタッチ"), SerializeField] private BlockArray blockArray;    

    [System.Serializable]
    public class BlockArray
    {
        public GameObject[] blockPrefab;
    }

    [Header("ブロック数(横)")] public int xNum;
    [Header("ブロック数(奥)")] public int zNum;
    [Header("スタート位置")] public Vector3 startPos = new Vector3(0, 0, 0);
    [Header("ブロックの大きさ")] public float unitWidth = 1;
    [Header("クリックでステージ生成")] public bool create;
    [Space]
    public List<GameObject> blocks;

    private void OnValidate()
    {
        switch (blockType)
        {
            case BlockType.Single:
                ResizeBlocks(1);
                break;
            case BlockType.Cross:
                ResizeBlocks(2);
                break;
            case BlockType.Random:
                ResizeBlocks(preNum); 
                break;
        }

        if (create)
        {
            create = false;

            switch (blockType)
            {
                case BlockType.Single:
                    SingleCreate();
                    break;
                case BlockType.Cross:
                    CrossCreate();
                    break;
                case BlockType.Random:
                    RundomCreate();
                    break;
            }

        }
    }

    private void ResizeBlocks(int size)
    {
        if (blockArray.blockPrefab == null || blockArray.blockPrefab.Length != size)
        {
            blockArray.blockPrefab = new GameObject[size];
        }
    }


    private void SingleCreate()
    {
        // 40は最大値
        zNum = Mathf.Min(40, zNum);
        xNum = Mathf.Min(40, xNum);
        int count = blocks.Count;

        var blockBase = new GameObject("Map");
        blockBase.transform.SetParent(transform);

        blocks.Clear();
        float posZ = startPos.z;
        float posY = startPos.y;
        float posX = startPos.x;

        //for (int z = 0; z < zNum; z++)
        //{
        //    for (int x = 0; x < xNum; x++)
        //    {
        //        GameObject prefab = blockArray.blockPrefab[0];

        //        GameObject birthObj = Instantiate(prefab);
        //        birthObj.transform.position = new Vector3(posX, posY, posZ);
        //        birthObj.transform.SetParent(blockBase.transform);
        //        birthObj.name = "Block" + " x:" + x + " z:" + z;

        //        blocks.Add(birthObj);
        //        posX += unitWidth;
        //    }
        //    posZ += unitWidth;
        //    posX = startPos.x;
        //}

        for (int z = 0; z < zNum; z++)
        {
            for (int x = 0; x < xNum; x++)
            {
                GameObject prefab = blockArray.blockPrefab[0];

                GameObject containerObj = new GameObject("Container" + " x:" + x + " z:" + z);
                containerObj.transform.SetParent(blockBase.transform);
                containerObj.transform.position = new Vector3(posX, posY, posZ);

                GameObject birthObj = Instantiate(prefab, containerObj.transform);
                birthObj.name = "Block" + " x:" + x + " z:" + z;

                blocks.Add(birthObj);
                posX += unitWidth;
            }
            posZ += unitWidth;
            posX = startPos.x;
        }
    }

    private void CrossCreate()
    {
        // 40は最大値
        zNum = Mathf.Min(40, zNum);
        xNum = Mathf.Min(40, xNum);
        int count = blocks.Count;

        var blockBase = new GameObject("Blocks");
        blockBase.transform.SetParent(transform);

        blocks.Clear();
        float posZ = startPos.z;
        float posY = startPos.y;
        float posX = startPos.x;

        for (int z = 0; z < zNum; z++)
        {
            for (int x = 0; x < xNum; x++)
            {
                int index = 0;

                if(z % 2 == 0) // zが偶数
                {
                    if (x % 2 == 0)
                        index = 0;
                    else
                        index = 1;
                }
                else // 奇数
                {
                    if (x % 2 == 0)
                        index = 1;
                    else
                        index = 0;
                }

                GameObject prefab = blockArray.blockPrefab[index];

                GameObject birthObj = Instantiate(prefab);
                birthObj.transform.position = new Vector3(posX, posY, posZ);
                birthObj.transform.SetParent(blockBase.transform);
                birthObj.name = "Block" + " x:" + x + " z:" + z;

                blocks.Add(birthObj);
                posX += unitWidth;
            }
            posZ += unitWidth;
            posX = startPos.x;
        }
    }

    private void RundomCreate()
    {
        // 40は最大値
        zNum = Mathf.Min(40, zNum);
        xNum = Mathf.Min(40, xNum);
        int count = blocks.Count;

        var blockBase = new GameObject("Block");
        blockBase.transform.SetParent(transform);

        blocks.Clear();
        float posZ = startPos.z;
        float posY = startPos.y;
        float posX = startPos.x;

        for (int z = 0; z < zNum; z++)
        {
            for (int x = 0; x < xNum; x++)
            {
                int index = Random.Range(0, preNum);


                GameObject prefab = blockArray.blockPrefab[index];

                GameObject birthObj = Instantiate(prefab);
                birthObj.transform.position = new Vector3(posX, posY, posZ);
                birthObj.transform.SetParent(blockBase.transform);
                birthObj.name = "Block" + " x:" + x + " z:" + z;

                blocks.Add(birthObj);
                posX += unitWidth;
            }
            posZ += unitWidth;
            posX = startPos.x;
        }
    }
}