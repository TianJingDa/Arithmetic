using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveFileFrameWrapper : GuiFrameWrapper
{
    private bool onlyWrong;
    private List<QuentionInstance> onlyWrongList;

    private SaveFileInstance content;
    private GameObject saveFileDetailBg;
    private GameObject onlyWrongImage;
    private Text saveFileDetailTime;
    private Text saveFileDetailAmount;
    private Text saveFileDetailAccuracy;
    private InfiniteList saveFileDetailGrid;

    void Start()
    {
        id = GuiFrameID.SaveFileFrame;
        Init();
        content = GameManager.Instance.CurSaveFileInstance;
        saveFileDetailTime.text = string.Format(saveFileDetailTime.text, content.timeCost.ToString("f1"));
        saveFileDetailAmount.text = string.Format(saveFileDetailAmount.text, content.qInstancList.Count);
        saveFileDetailAccuracy.text = string.Format(saveFileDetailAccuracy.text, content.accuracy);
        onlyWrongList = content.qInstancList.FindAll(FindWrong);
        onlyWrong = false;
        RefreshSettlementGrid();
        CommonTool.GuiVerticalMove(saveFileDetailBg, Screen.height, MoveID.LeftOrDown, canvasGroup, true);
    }

    protected override void OnStart(Dictionary<string, GameObject> gameObjectDict)
    {
        saveFileDetailBg                = gameObjectDict["SaveFileDetailBg"];
        onlyWrongImage                  = gameObjectDict["OnlyWrongImage"];
        saveFileDetailTime              = gameObjectDict["SaveFileDetailTime"].GetComponent<Text>();
        saveFileDetailAmount            = gameObjectDict["SaveFileDetailAmount"].GetComponent<Text>();
        saveFileDetailAccuracy          = gameObjectDict["SaveFileDetailAccuracy"].GetComponent<Text>();
        saveFileDetailGrid              = gameObjectDict["SaveFileDetailGrid"].GetComponent<InfiniteList>();
    }

    protected override void OnButtonClick(Button btn)
    {
        base.OnButtonClick(btn);
        switch (btn.name)
        {
            case "ShareBtn":
                GameManager.Instance.CurShareInstance = new ShareInstance(ShareID.SaveFile);
                GameManager.Instance.SwitchWrapper(GuiFrameID.ShareFrame, true);
                break;
            case "OnlyWrongBtn":
                onlyWrong = !onlyWrong;
                RefreshSettlementGrid();
                break;
            case "SaveFileDetailClosedBtn":
                CommonTool.GuiVerticalMove(saveFileDetailBg, Screen.height, MoveID.LeftOrDown, canvasGroup, false, () => GameManager.Instance.SwitchWrapper(GuiFrameID.None));
                break;
            case "AchievementBtn":
                AchievementInstance instance = GameManager.Instance.GetAchievement(content.achievementName);
                GameManager.Instance.CurAchievementInstance = instance;
                GameManager.Instance.CurShareInstance = new ShareInstance(ShareID.Achievement);
                GameManager.Instance.SwitchWrapper(GuiFrameID.ShareFrame, true);
                break;
            default:
                MyDebug.LogYellow("Can not find Button: " + btn.name);
                break;
        }
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
            dataList = new ArrayList(content.qInstancList);
        }
        saveFileDetailGrid.InitList(dataList, GuiItemID.QuestionItem);
    }
}
