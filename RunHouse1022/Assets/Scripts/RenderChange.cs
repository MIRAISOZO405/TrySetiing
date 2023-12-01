using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerEnums;

public class RenderChange : MonoBehaviour
{
    private Transform house;
    private Transform apart;
    private Transform mansion;
    private Transform towerMansion;

    private Transform currentTrans;
    private GameObject levelManager;

    private void Start()
    {
        house = GameObject.Find("RenderHouse").gameObject.transform;
        apart = GameObject.Find("RenderApart").gameObject.transform;
        mansion = GameObject.Find("RenderMansion").gameObject.transform;
        towerMansion = GameObject.Find("RenderTowerMansion").gameObject.transform;

        if (!house || !apart || !mansion || !towerMansion)
        {
            Debug.Log("ÉåÉìÉ_Å\ÉÇÉfÉãÇ™å©Ç¬Ç©ÇËÇ‹ÇπÇÒ");
        }

        levelManager = GameObject.Find("LevelManager").gameObject;
        if (!levelManager)
        {
            Debug.Log("levelManagerÇ™å©Ç¬Ç©ÇËÇ‹ÇπÇÒ");
        }

        currentTrans = house;
    }

    public void ModelChange(PlayerLevel lv)
    {
        switch (lv)
        {
            case PlayerLevel.House:
                currentTrans = house;
                break;
            case PlayerLevel.Apart:
                currentTrans = apart;
                break;
            case PlayerLevel.Mansion:
                currentTrans = mansion;
                break;
            case PlayerLevel.TowerMansion:
                currentTrans = towerMansion;
                break;
        }
        this.transform.position = new Vector3(currentTrans.position.x, transform.position.y, transform.position.z);
    }

    public void CurrentModelChange()
    {
        PlayerLevel lv = levelManager.GetComponent<LevelManager>().GetLevel();
        ModelChange(lv);
    }
}
