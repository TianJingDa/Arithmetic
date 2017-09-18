﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 结算界面
/// </summary>
public class SettlementFrameWrapper : GuiFrameWrapper
{
    private float timeCost;
    private string symbol;
    private List<List<int>> resultList;
    private List<List<int>> onlyWrongList;

    private Text settlementTime_Text;
    private Text settlementAmount_Text;
    private Text settlementAccuracy_Text;
    private InfiniteList settlementGrid;
    private GameObject achievementDetailBgInSettlement;

    void Start () 
	{
        id = GuiFrameID.SettlementFrame;
        Init();
        InitSettlement();
        ShowAchievement();
    }

    protected override void OnStart(Dictionary<string, GameObject> GameObjectDict)
    {
        settlementGrid                  = GameObjectDict["SettlementGrid"].GetComponent<InfiniteList>();
        settlementTime_Text             = GameObjectDict["SettlementTime_Text"].GetComponent<Text>();
        settlementAmount_Text           = GameObjectDict["SettlementAmount_Text"].GetComponent<Text>();
        settlementAccuracy_Text         = GameObjectDict["SettlementAccuracy_Text"].GetComponent<Text>();
        achievementDetailBgInSettlement = GameObjectDict["AchievementDetailBgInSettlement"];
    }

    protected override void OnButtonClick(Button btn)
    {
        base.OnButtonClick(btn);
        switch (btn.name)
        {
            case "AchievementDetailBgInSettlement":
                achievementDetailBgInSettlement.SetActive(false);
                if (false)
                {
                    ShowAchievement();
                }
                break;
            case "OnlyWrongBtn":
                Debug.Log("OnlyWrong!!!");
                break;
            case "Settlement2CategoryFrameBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.SettlementFrame, GuiFrameID.CategoryFrame);
                break;
            case "Settlement2StartFrameBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.SettlementFrame, GuiFrameID.StartFrame);
                break;
            default:
                MyDebug.LogYellow("Can not find Button: " + btn.name);
                break;
        }
    }

    private void InitSettlement()
    {
        GameManager.Instance.GetSettlementParameter(out timeCost, out resultList, out symbol);
        settlementTime_Text.text = settlementTime_Text.text.Replace("{0}", timeCost.ToString("f1"));
        settlementAmount_Text.text = settlementAmount_Text.text.Replace("{0}", resultList.Count.ToString());
        onlyWrongList = resultList.FindAll(x => x[x.Count - 1] != x[x.Count - 2]);
        settlementAccuracy_Text.text = settlementAccuracy_Text.text.Replace("{0}", ((float)onlyWrongList.Count * 100 / (float)resultList.Count).ToString("f1"));
        ArrayList dataList = new ArrayList();
        for (int i = 0; i < resultList.Count; i++)
        {
            QuentionInstance questionInstance = new QuentionInstance();
            questionInstance.index = (i + 1).ToString();
            questionInstance.symbol = symbol;
            questionInstance.instance = resultList[i];
            dataList.Add(questionInstance);
        }
        settlementGrid.InitList(dataList, "QuestionItem");
    }

    private void ShowAchievement()
    {
        achievementDetailBgInSettlement.SetActive(true);
    }
}
