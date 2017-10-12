using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 结算界面
/// </summary>
public class SettlementFrameWrapper : GuiFrameWrapper
{
    private bool onlyWrong;

    private Text settlementTime;
    private Text settlementAmount;
    private Text settlementAccuracy;
    private GameObject achievementDetailBgInSettlement;
    private InfiniteList settlementGrid;
    private SaveFileInstance curSaveFileInstance;
    private List<QuentionInstance> onlyWrongList;
    private List<QuentionInstance> allInstanceList;

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
        settlementTime                  = GameObjectDict["SettlementTime"].GetComponent<Text>();
        settlementAmount                = GameObjectDict["SettlementAmount"].GetComponent<Text>();
        settlementAccuracy              = GameObjectDict["SettlementAccuracy"].GetComponent<Text>();
        achievementDetailBgInSettlement = GameObjectDict["AchievementDetailBgInSettlement"];
    }

    protected override void OnButtonClick(Button btn)
    {
        base.OnButtonClick(btn);
        switch (btn.name)
        {
            case "AchievementDetailBgInSettlement"://容易误点
                achievementDetailBgInSettlement.SetActive(false);
                if (false)
                {
                    ShowAchievement();
                }
                break;
            case "CurAchievementBtn":
                ShowAchievement();
                break;
            case "OnlyWrongBtn":
                onlyWrong = !onlyWrong;
                RefreshSettlementGrid();
                break;
            case "ShareBtn":
                MyDebug.LogYellow("ShareBtn");
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
        curSaveFileInstance = GameManager.Instance.CurSaveFileInstance;
        settlementTime.text = string.Format(settlementTime.text, curSaveFileInstance.timeCost.ToString("f1"));
        settlementAmount.text = string.Format(settlementAmount.text, curSaveFileInstance.qInstancList.Count);
        settlementAccuracy.text = string.Format(settlementAccuracy.text, curSaveFileInstance.accuracy);
        allInstanceList = curSaveFileInstance.qInstancList;
        onlyWrongList = allInstanceList.FindAll(FindWrong);
        onlyWrong = false;
        RefreshSettlementGrid();
    }

    private bool FindWrong(QuentionInstance questionInstance)
    {
        int count = questionInstance.instance.Count;
        return questionInstance.instance[count - 1] != questionInstance.instance[count - 2];
    }

    private void RefreshSettlementGrid()
    {
        ArrayList dataList;
        if (onlyWrong)
        {
            dataList = new ArrayList(onlyWrongList);
        }
        else
        {
            dataList = new ArrayList(allInstanceList);
        }
        settlementGrid.InitList(dataList, "QuestionItem");
    }
    private void ShowAchievement()
    {
        achievementDetailBgInSettlement.SetActive(true);
    }
}
