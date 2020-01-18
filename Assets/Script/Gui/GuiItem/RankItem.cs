﻿using System;
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
		rankIndex.text = content.rank.ToString();
		rankUserName.text = content.name;
        string timeCost = GameManager.Instance.GetMutiLanguage("Text_90006");
		rankTimeCost.text = string.Format(timeCost, content.timelast.ToString("f1"));
        string accuracy = GameManager.Instance.GetMutiLanguage("Text_90007");
        rankAccuracy.text = string.Format(accuracy, content.accuracy.ToString("f1"));
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
		WWWForm form = new WWWForm();
		form.AddField("userId", GameManager.Instance.UserID);
		form.AddField("jwttoken", GameManager.Instance.Token);
		form.AddField("model", (int)GameManager.Instance.CurCategoryInstance.patternID + 1);
		form.AddField("rankId", content.id);
		GameManager.Instance.GetRankDetail(form, OnGetRankDetailSucceed, OnGetRankDetailFail);
    }

	private void OnGetRankDetailSucceed(string data)
	{
		SaveFileInstance instance = JsonUtility.FromJson<SaveFileInstance>(data);
		if(instance != null)
		{
			GameManager.Instance.CurSaveFileInstance = instance;
			GameManager.Instance.SwitchWrapper(GuiFrameID.SaveFileFrame, true);
		}
	}

	private void OnGetRankDetailFail(string message)
	{
		GameManager.Instance.CurCommonTipInstance = new CommonTipInstance(CommonTipID.Splash, message);
		GameManager.Instance.SwitchWrapper(GuiFrameID.CommonTipFrame, true);
	}
}

[Serializable]
public class RankInstance
{
	public int rank;//排名
	public int id;//用于获取详情
	public string userId;
	public string name;
	public float timelast;
	public float accuracy;
	public float score;
}
