using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMaker : MonoBehaviour
{
    [Header("�I�u�W�F�N�g�����m��(100%��)")] public int probability;
    [Header("�g�p����v���n�u�̎��")] public int preNum = 3;
    [Header("�g�p����Block���A�^�b�`"), SerializeField] private ObjectArray objectArray;

    [System.Serializable]
    public class ObjectArray
    {
        public GameObject[] objectPrefab;
    }

    [Header("�u���b�N��(��)")] public int xNum;
    [Header("�u���b�N��(��)")] public int zNum;
    [Header("�X�^�[�g�ʒu")] public Vector3 startPos = new Vector3(0, 0, 0);
    [Header("�u���b�N�̑傫��")] public float unitWidth = 1;
    [Header("�N���b�N�ŃI�u�W�F�N�g����")] public bool create;
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

            // 40�͍ő�l
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
                    // 0�`99�̃����_���Ȑ����擾���āA���̐���probability�����̏ꍇ�̂݃I�u�W�F�N�g�𐶐�
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
