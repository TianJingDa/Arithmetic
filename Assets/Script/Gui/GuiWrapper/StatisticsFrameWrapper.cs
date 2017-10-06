using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// 统计数据界面
/// </summary>
public class StatisticsFrameWrapper : GuiFrameWrapper
{
    private Text totelTimeImg_Text2;
    private Text totelGameImg_Text2;
    private GameObject achievementWin;
    private GameObject saveFileWin;
    private GameObject saveFileDetailBg;
    private GameObject deleteSaveFileBg;
    //private GameObject achievementDetailBgInSaveFile;
    private GameObject achievementDetailBgInStatistics;
    private InfiniteList achievementGrid;
    private InfiniteList saveFileGrid;
    void Start () 
	{
        id = GuiFrameID.StatisticsFrame;
        Init();
        int totalTime = (int)GameManager.Instance.TotalTime;
        TimeSpan ts = new TimeSpan(0, 0, totalTime);
        totelTimeImg_Text2.text = string.Format(totelTimeImg_Text2.text, ts.Hours, ts.Minutes, ts.Seconds);
        totelGameImg_Text2.text = string.Format(totelGameImg_Text2.text, GameManager.Instance.TotalGame);
    }

    protected override void OnStart(Dictionary<string, GameObject> GameObjectDict)
    {
        achievementWin                  = GameObjectDict["AchievementWin"];
        saveFileWin                     = GameObjectDict["SaveFileWin"];
        saveFileGrid                    = GameObjectDict["SaveFileGrid"].GetComponent<InfiniteList>();
        achievementGrid                 = GameObjectDict["AchievementGrid"].GetComponent<InfiniteList>();
        saveFileDetailBg                = GameObjectDict["SaveFileDetailBg"];
        deleteSaveFileBg                = GameObjectDict["DeleteSaveFileBg"];
        totelTimeImg_Text2              = GameObjectDict["TotelTimeImg_Text2"].GetComponent<Text>();
        totelGameImg_Text2              = GameObjectDict["TotelGameImg_Text2"].GetComponent<Text>();
        //achievementDetailBgInSaveFile   = GameObjectDict["AchievementDetailBgInSaveFile"];
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
                achievementWin.SetActive(true);
                InitAchievementList();
                break;
            case "AchievementDetailBg":
                achievementDetailBgInStatistics.SetActive(false);
                break;
            //case "AchievementDetailBgInSaveFile":
            //    achievementDetailBgInSaveFile.SetActive(false);
            //    break;
            case "DeleteSaveFileBg":
            case "DeleteCancelBtn":
                deleteSaveFileBg.SetActive(false);
                break;
            case "Save2StatisticsFrameBtn":
                GameManager.Instance.CurAction = null;
                saveFileWin.SetActive(false);
                break;
            case "SaveFileBtn":
                saveFileWin.SetActive(true);
                GameManager.Instance.CurAction = InitSaveFileList;
                InitSaveFileList();
                break;
            case "SaveFileDetai2SaveFileWinBtn":
                saveFileDetailBg.SetActive(false);
                break;
            case "SaveFileDetai2StatisticsFrameBtn":
                GameManager.Instance.CurAction = null;
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
        ArrayList dataList = new ArrayList();
        for(int i = 0; i < 8; i++)
        {
            AchievementInstance instance = new AchievementInstance();
            instance.achievementKey = i.ToString();
            instance.detail = i.ToString();
            dataList.Add(instance);
        }
        achievementGrid.InitList(dataList, "AchievementItem", achievementDetailBgInStatistics);
    }

    private void InitSaveFileList()
    {
        List<SaveFileInstance> recordList = GameManager.Instance.ReadRecord();
        ArrayList dataList = new ArrayList(recordList);
        saveFileGrid.InitList(dataList, "SaveFileItem" , saveFileDetailBg, deleteSaveFileBg);
    }
}
