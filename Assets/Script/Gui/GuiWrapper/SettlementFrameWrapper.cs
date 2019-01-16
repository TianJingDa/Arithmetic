using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using cn.sharesdk.unity3d;

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
    private GameObject onlyWrongImage;
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
        InitAchievement();
    }

    protected override void OnStart(Dictionary<string, GameObject> gameObjectDict)
    {
        settlementGrid                              = gameObjectDict["SettlementGrid"].GetComponent<InfiniteList>();
        settlementTime                              = gameObjectDict["SettlementTime"].GetComponent<Text>();
        settlementAmount                            = gameObjectDict["SettlementAmount"].GetComponent<Text>();
        settlementAccuracy                          = gameObjectDict["SettlementAccuracy"].GetComponent<Text>();
        achievementDetailMainTitleInSettlement      = gameObjectDict["AchievementDetailMainTitleInSettlement"].GetComponent<Text>();
        achievementDetailSubTitleInSettlement       = gameObjectDict["AchievementDetailSubTitleInSettlement"].GetComponent<Text>();
        achievementDetailFinishTimeInSettlement     = gameObjectDict["AchievementDetailFinishTimeInSettlement"].GetComponent<Text>();
        achievementDetailBgInSettlement             = gameObjectDict["AchievementDetailBgInSettlement"];
        achievementDetailImageInSettlement          = gameObjectDict["AchievementDetailImageInSettlement"].GetComponent<Image>();
        onlyWrongImage                              = gameObjectDict["OnlyWrongImage"];
    }

    protected override void OnButtonClick(Button btn)
    {
        base.OnButtonClick(btn);
        switch (btn.name)
        {
            case "AchievementDetail2SettlementBtn":
                achievementDetailBgInSettlement.SetActive(false);
                break;
            case "OnlyWrongBtn":
                onlyWrong = !onlyWrong;
                RefreshSettlementGrid();
                break;
            case "Settlement2CategoryFrameBtn":
				GameManager.Instance.SwitchWrapperWithMove(GameManager.Instance.CompetitionGUI,MoveID.RightOrUp,false);
                break;
            case "Settlement2StartFrameBtn":
                GameManager.Instance.SwitchWrapperWithScale(GuiFrameID.StartFrame,false);
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
        onlyWrongImage.SetActive(onlyWrong);
        ArrayList dataList;
        if (onlyWrong)
        {
            dataList = new ArrayList(onlyWrongList);
        }
        else
        {
            dataList = new ArrayList(allInstanceList);
        }
        settlementGrid.InitList(dataList, GuiItemID.QuestionItem);
    }
    private void InitAchievement()
    {
		if (!string.IsNullOrEmpty(GameManager.Instance.CurAchievementName) && GameManager.Instance.CompetitionGUI == GuiFrameID.ChapterFrame)
        {
            achievementDetailBgInSettlement.SetActive(true);
            AchievementInstance instance = GameManager.Instance.GetAchievement(GameManager.Instance.CurAchievementName);
            achievementDetailImageInSettlement.sprite = GameManager.Instance.GetSprite(instance.imageIndex);
            achievementDetailMainTitleInSettlement.text = GameManager.Instance.GetMutiLanguage(instance.mainTitleIndex);
            achievementDetailSubTitleInSettlement.text = GameManager.Instance.GetMutiLanguage(instance.subTitleIndex);
            achievementDetailFinishTimeInSettlement.text = GetFinishTime(instance.finishTime);
        }
    }
    private string GetFinishTime(string time)
    {
        StringBuilder newTime = new StringBuilder(time.Substring(0, 8));
        newTime.Insert(4, ".");
        newTime.Insert(7, ".");
        return newTime.ToString();
    }
}
