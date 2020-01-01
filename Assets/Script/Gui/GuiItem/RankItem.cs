using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RankItem : Item, IPointerClickHandler
{
    private RankInstance content;

    private Text rankIndex;
    private Text rankUserName;
    private Text rankTimeCost;
    private Text rankAccuracy;

    protected override void InitPrefabItem(object data)
    {
        content = data as RankInstance;
        if (content == null)
        {
            MyDebug.LogYellow("RankInstance is null!!");
            return;
        }

        Init();
        rankIndex.text = content.index.ToString();
        rankUserName.text = content.userName;
        string timeCost = GameManager.Instance.GetMutiLanguage("Text_90006");
        rankTimeCost.text = string.Format(timeCost, content.saveFile.timeCost.ToString("f1"));
        string accuracy = GameManager.Instance.GetMutiLanguage("Text_90007");
        rankAccuracy.text = string.Format(accuracy, content.saveFile.accuracy.ToString("f1"));
    }

    protected override void OnStart(Dictionary<string, GameObject> gameObjectDict)
    {
        rankIndex       = gameObjectDict["RankIndex"].GetComponent<Text>();
        rankUserName    = gameObjectDict["RankUserName"].GetComponent<Text>();
        rankTimeCost    = gameObjectDict["RankTimeCost"].GetComponent<Text>();
        rankAccuracy    = gameObjectDict["RankAccuracy"].GetComponent<Text>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.CurSaveFileInstance = content.saveFile;
        GameManager.Instance.SwitchWrapper(GuiFrameID.SaveFileFrame, true);
    }
}

[Serializable]
public class RankInstance
{
    public int index;
    public string userName;
    public SaveFileInstance saveFile;
}
