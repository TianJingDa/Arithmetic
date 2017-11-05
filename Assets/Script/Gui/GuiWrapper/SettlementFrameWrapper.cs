using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
/// <summary>
/// 结算界面
/// </summary>
public class SettlementFrameWrapper : GuiFrameWrapper
{
    private bool onlyWrong;

    private Text settlementTime;
    private Text settlementAmount;
    private Text settlementAccuracy;
    private Text achievementDetailMainTitleInSettlement;
    private Text achievementDetailSubTitleInSettlement;
    private Text achievementDetailFinishTimeInSettlement;
    private Image achievementDetailImageInSettlement;
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
        settlementGrid                              = GameObjectDict["SettlementGrid"].GetComponent<InfiniteList>();
        settlementTime                              = GameObjectDict["SettlementTime"].GetComponent<Text>();
        settlementAmount                            = GameObjectDict["SettlementAmount"].GetComponent<Text>();
        settlementAccuracy                          = GameObjectDict["SettlementAccuracy"].GetComponent<Text>();
        achievementDetailMainTitleInSettlement      = GameObjectDict["AchievementDetailMainTitleInStatistics"].GetComponent<Text>();
        achievementDetailSubTitleInSettlement       = GameObjectDict["AchievementDetailSubTitleInStatistics"].GetComponent<Text>();
        achievementDetailFinishTimeInSettlement     = GameObjectDict["AchievementDetailFinishTimeInStatistics"].GetComponent<Text>();
        achievementDetailBgInSettlement             = GameObjectDict["AchievementDetailBgInSettlement"];
        achievementDetailImageInSettlement          = GameObjectDict["AchievementDetailImageInStatistics"].GetComponent<Image>();
    }

    protected override void OnButtonClick(Button btn)
    {
        base.OnButtonClick(btn);
        switch (btn.name)
        {
            case "AchievementDetailBgInSettlement"://容易误点
                achievementDetailBgInSettlement.SetActive(false);
                break;
            case "AchievementDetailShareBtnInSettlement":
                OnShareBtn();
                break;
            case "CurAchievementBtn":
                ShowAchievement();
                break;
            case "OnlyWrongBtn":
                onlyWrong = !onlyWrong;
                RefreshSettlementGrid();
                break;
            case "ShareBtnInSettlement":
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
        if (string.IsNullOrEmpty(curSaveFileInstance.achievementName)) return;
        achievementDetailBgInSettlement.SetActive(true);
        AchievementInstance instance = GameManager.Instance.GetAchievement(curSaveFileInstance.achievementName);
        achievementDetailImageInSettlement.sprite = GameManager.Instance.GetSprite(instance.imageIndex);
        achievementDetailMainTitleInSettlement.text = GameManager.Instance.GetMutiLanguage(instance.mainTitleIndex);
        achievementDetailSubTitleInSettlement.text = GameManager.Instance.GetMutiLanguage(instance.subTitleIndex);
        achievementDetailFinishTimeInSettlement.text = GetFinishTime(instance.fileName);
    }
    private string GetFinishTime(string time)
    {
        StringBuilder newTime = new StringBuilder(time.Substring(0, 8));
        newTime.Insert(4, ".");
        newTime.Insert(7, ".");
        return newTime.ToString();
    }
    private void OnShareBtn()
    {
        MyDebug.LogYellow("ShareBtn");
    }
}
