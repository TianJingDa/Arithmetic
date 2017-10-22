﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour 
{
    protected void Init()
    {
        CommonTool.InitText(gameObject);
        CommonTool.InitImage(gameObject);
        Dictionary<string, GameObject> GameObjectDict = CommonTool.InitGameObjectDict(gameObject);
        GetComponent<Item>().OnStart(GameObjectDict);
    }
    protected virtual void OnStart(Dictionary<string, GameObject> GameObjectDict)
    {

    }
    protected virtual void InitPrefabItem(object data)
    {

    }
    protected virtual void InitDetailWin(GameObject detailWin)
    {

    }
    protected virtual void InitDeleteWin(GameObject deleteWin)
    {

    }
}
