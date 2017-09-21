using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 统计数据界面
/// </summary>
public class StatisticsFrameWrapper : GuiFrameWrapper
{
    private bool onlyWrong;

    private GameObject achievementWin;
    private GameObject saveFileWin;
    private GameObject saveFileDetailBg;
    private GameObject achievementDetailBgInSaveFile;
    private GameObject achievementDetailBgInStatistics;
    private InfiniteList achievementGrid;
    private InfiniteList saveFileGrid;
    void Start () 
	{
        id = GuiFrameID.StatisticsFrame;
        Init();
        onlyWrong = false;
    }

    protected override void OnStart(Dictionary<string, GameObject> GameObjectDict)
    {
        achievementWin                  = GameObjectDict["AchievementWin"];
        saveFileWin                     = GameObjectDict["SaveFileWin"];
        saveFileGrid                    = GameObjectDict["SaveFileGrid"].GetComponent<InfiniteList>();
        achievementGrid                 = GameObjectDict["AchievementGrid"].GetComponent<InfiniteList>();
        saveFileDetailBg                = GameObjectDict["SaveFileDetailBg"];
        achievementDetailBgInSaveFile   = GameObjectDict["AchievementDetailBgInSaveFile"];
        achievementDetailBgInStatistics = GameObjectDict["AchievementDetailBgInStatistics"];
    }

    protected override void OnButtonClick(Button btn)
    {
        base.OnButtonClick(btn);
        switch (btn.name)
        {
            case "Achievement2StartFrameBtn":
            case "Save2StartFrameBtn":
            case "Statistics2StartFrameBtn":
                GameManager.Instance.SwitchWrapper(GuiFrameID.StatisticsFrame, GuiFrameID.StartFrame);
                break;
            case "Achievement2StatisticsFrameBtn":
                achievementWin.SetActive(false);
                break;
            case "AchievementBtn":
                InitAchievementList();
                break;
            case "AchievementDetailBg":
                achievementDetailBgInStatistics.SetActive(false);
                break;
            case "AchievementDetailBgInSaveFile":
                achievementDetailBgInSaveFile.SetActive(false);
                break;
            case "OnlyWrongBtnInStatistics":
                onlyWrong = !onlyWrong;
                RefreshSettlementGrid();
                break;
            case "CurAchievementBtnInStatistics":
                achievementDetailBgInSaveFile.SetActive(true);
                ShowAchievement();
                break;
            case "Save2StatisticsFrameBtn":
                saveFileWin.SetActive(false);
                break;
            case "SaveFileBtn":
                saveFileWin.SetActive(true);
                InitSaveFileList();
                break;
            case "SaveFileDetai2SaveFileWinBtn":
                saveFileDetailBg.SetActive(false);
                break;
            case "SaveFileDetai2StatisticsFrameBtn":
                saveFileDetailBg.SetActive(false);
                saveFileWin.SetActive(false);
                break;
            default:
                MyDebug.LogYellow("Can not find Button:" + btn.name);
                break;
        }
    }

    private void InitAchievementList()
    {
        achievementWin.SetActive(true);
        ArrayList dataList = new ArrayList();
        for(int i = 0; i < 8; i++)
        {
            AchievementInstance instance = new AchievementInstance();
            instance.title = i.ToString();
            instance.detail = i.ToString();
            dataList.Add(instance);
        }
        achievementGrid.InitList(dataList, "AchievementItem", achievementDetailBgInStatistics);
    }

    private void InitSaveFileList()
    {
        Dictionary<string, List<QuentionInstance>> recordDict = GameManager.Instance.ReadRecord();
        ArrayList dataList = new ArrayList();
        foreach(string key in recordDict.Keys)
        {
            SaveFileInstance saveFileInstance = new SaveFileInstance();
            saveFileInstance.title = key;
            saveFileInstance.instance = recordDict[key];
            dataList.Add(saveFileInstance);
        }
        saveFileGrid.InitList(dataList, "SaveFileItem" , saveFileDetailBg);
    }

    private void RefreshSettlementGrid()
    {

    }
    private void ShowAchievement()
    {

    }

}
