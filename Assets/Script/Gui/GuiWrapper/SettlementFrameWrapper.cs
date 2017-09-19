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

    private Text settlementTime_Text;
    private Text settlementAmount_Text;
    private Text settlementAccuracy_Text;
    private GameObject achievementDetailBgInSettlement;
    private InfiniteList settlementGrid;
    private List<QuentionInstance> resultList;
    private List<QuentionInstance> onlyWrongList;

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
                onlyWrong = !onlyWrong;
                RefreshSettlementGrid();
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
        float timeCost;
        string symbol;
        List<List<int>> resultTempList;
        GameManager.Instance.GetSettlementParameter(out timeCost, out resultTempList, out symbol);
        string count = resultTempList.Count.ToString();
        settlementTime_Text.text = settlementTime_Text.text.Replace("{0}", timeCost.ToString("f1"));
        settlementAmount_Text.text = settlementAmount_Text.text.Replace("{0}", count);
        resultList = new List<QuentionInstance>();
        for (int i = 0; i < resultTempList.Count; i++)
        {
            QuentionInstance questionInstance = new QuentionInstance();
            questionInstance.index = (i + 1).ToString().PadLeft(count.Length, '0');
            questionInstance.symbol = symbol;
            questionInstance.instance = resultTempList[i];
            resultList.Add(questionInstance);
        }
        onlyWrongList = resultList.FindAll(FindWrong);
        settlementAccuracy_Text.text = settlementAccuracy_Text.text.Replace("{0}", (100 - ((float)onlyWrongList.Count * 100 / resultTempList.Count)).ToString("f1"));
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
            dataList = new ArrayList(resultList);
        }
        settlementGrid.InitList(dataList, "QuestionItem");
    }


    private void ShowAchievement()
    {
        achievementDetailBgInSettlement.SetActive(true);
    }
}
